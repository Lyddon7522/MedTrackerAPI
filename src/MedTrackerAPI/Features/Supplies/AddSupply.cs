using FluentValidation;
using MediatR;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Supplies;

public static class AddSupply
{
    public record Response (int SupplyId, string? Description, string? Manufacturer, string? PartNumber, string? LotNumber, int DeviceId);
    
    public class AddSupplyCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public string? Manufacturer { get; set; }
        public required string PartNumber { get; set; }
        public required string LotNumber { get; set; }
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