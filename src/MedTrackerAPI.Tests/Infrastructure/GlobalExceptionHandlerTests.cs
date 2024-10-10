using MedTrackerAPI.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace MedTrackerAPI.Tests.Infrastructure
{
    [TestFixture]
    public class GlobalExceptionHandlerTests
    {
        private Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
        private GlobalExceptionHandler _exceptionHandler;
        private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            _exceptionHandler = new GlobalExceptionHandler(_loggerMock.Object);
        }

        [Test]
        public async Task TryHandleAsync_ShouldLogErrorAndReturnProblemDetails()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseBodyStream = new MemoryStream();
            httpContext.Response.Body = responseBodyStream;
            var exception = new Exception("Test exception");
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, cancellationToken);

            // Assert
            Assert.That(result, Is.True);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test exception")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);

            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            responseBodyStream.Position = 0;

            var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(responseBodyStream, JsonSerializerOptions, cancellationToken);

            Assert.That(problemDetails, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(problemDetails.Title, Is.EqualTo("Internal Server Error"));
                Assert.That(problemDetails.Type, Is.EqualTo("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"));
            });
        }
    }
}