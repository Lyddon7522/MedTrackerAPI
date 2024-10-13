using FluentValidation.TestHelper;
using MedTrackerAPI.Features.Users;
using MedTrackerAPI.Authentication;
using MedTrackerAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using static MedTrackerAPI.Features.Users.RegisterUser;

namespace MedTrackerAPI.Tests.Features.Users;

[TestFixture]
public class RegisterUserTests
{
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoEmail_ThenIsNotValid(string? value) =>
        
        new RegisterUserCommandValidator().TestValidate(new RegisterUserCommand
        {
            Email = value
        }).ShouldHaveValidationErrorFor(x => x.Email);
    
    [TestCase("invalidEmail")]
    public void GivenInvalidEmail_ThenIsNotValid(string value) =>
        new RegisterUserCommandValidator().TestValidate(new RegisterUserCommand
        {
            Email = value
        }).ShouldHaveValidationErrorFor(x => x.Email);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPassword_ThenIsNotValid(string? value) =>
        new RegisterUserCommandValidator().TestValidate(new RegisterUserCommand
        {
            Password = value
        }).ShouldHaveValidationErrorFor(x => x.Password);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoName_ThenIsNotValid(string? value) =>
        new RegisterUserCommandValidator().TestValidate(new RegisterUserCommand
        {
            Name = value
        }).ShouldHaveValidationErrorFor(x => x.Name);
    
    [Test]
    public void GivenAllValidFields_ThenIsValid() =>
        new RegisterUserCommandValidator().TestValidate(new RegisterUserCommand
        {
            Email = "validemail@email.com",
            Password = "password123",
            Name = "Test Name"
        }).ShouldNotHaveAnyValidationErrors();
    
    [Test]
    public async Task RegistersNewUserWithIdentityProvider()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "newuser@example.com",
            Password = "newpassword123",
            Name = "New User"
        };
        var validator = new RegisterUserCommandValidator().TestValidate(command);
        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(x => x.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("mocked-uid");
        
        var options = new DbContextOptionsBuilder<MedTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new MedTrackerDbContext(options);

        var handler = new RegisterUserCommandHandler(context, mockAuthService.Object);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(validator.IsValid, Is.True);
        Assert.That(response, Is.Not.Null);
        Assert.That(response.IdentityId, Is.EqualTo("mocked-uid"));
    }

}