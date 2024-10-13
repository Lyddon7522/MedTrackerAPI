using MediatR;
using MedTrackerAPI.Authentication;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Users;

public static class RegisterUser
{
    //TODO: Add validator
    public record RegisterUserCommand(string Email, string Password, string Name) : IRequest<Response>;
    public record Response(Guid UserId, string Email, string Name, string IdentityId);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users", async (IMediator mediator, RegisterUserCommand command) =>
                {
                    var user = await mediator.Send(command);
                    return Results.Created("", user);
                }).WithTags("Users");
        }
    }

    public class RegisterUserCommandHandler(MedTrackerDbContext context, IAuthenticationService authenticationService) 
        : IRequestHandler<RegisterUserCommand, Response>
    {
        public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add Check for existing user email
            var identityId = await authenticationService.RegisterUserAsync(request.Email, request.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Name = request.Name,
                IdentityId = identityId
            };
            
            context.User.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            
            return new Response(user.UserId, user.Email, user.Name, user.IdentityId);
        }
    }
}