using FluentValidation;
using MediatR;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Supplies;

public static class AddSupply
{
    public record Response (int SupplyId, string? Description, string? Manufacturer, string? PartNumber, string? LotNumber, int DeviceId);
    
    public class AddSupplyCommandValidator : AbstractValidator<AddSupplyCommand>
    {
        public AddSupplyCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PartNumber).NotEmpty();
            RuleFor(x => x.LotNumber).NotEmpty();
            RuleFor(x => x.DeviceId).NotEmpty();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("supplies", async (IMediator mediator, AddSupplyCommand command) =>
            {
                var supply = await mediator.Send(command);

                return Results.Created("", supply);
            }).WithTags("Supplies")
            .AddEndpointFilter<ValidationFilter<AddSupplyCommand>>()
            .RequireAuthorization();
        }
    }
    
    public class AddSupplyCommand : IRequest<Response>
    {
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public string? PartNumber { get; set; }
        public string? LotNumber { get; set; }
        public int DeviceId { get; set; }
    }

    public class AddSupplyCommandHandler(MedTrackerDbContext context) : IRequestHandler<AddSupplyCommand, Response?>
    {
        public async Task<Response?> Handle(AddSupplyCommand request, CancellationToken cancellationToken)
        {
            var supply = new Supply
            {
                Description = request.Description,
                Manufacturer = request.Manufacturer,
                PartNumber = request.PartNumber,
                LotNumber = request.LotNumber,
                DeviceId = request.DeviceId
            };
            
            context.Supplies.Add(supply);
            await context.SaveChangesAsync(cancellationToken);
            
            return new Response(supply.Id, supply.Description, supply.Manufacturer, supply.PartNumber, supply.LotNumber, supply.DeviceId);
        }
    }
}