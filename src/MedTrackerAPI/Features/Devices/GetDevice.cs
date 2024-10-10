using MediatR;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Devices;

public static class GetDevice
{
    public record Response(int DeviceId, string? Description, string? Manufacturer, string? Model, string? PartNumber, string? LotNumber);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("devices/{deviceId}", async (IMediator mediator, int deviceId) =>
                {
                    var device = await mediator.Send(new GetDeviceQuery { DeviceId = deviceId });
                    
                    return device is null ? Results.NotFound() : Results.Ok(device);
                }).WithName("GetDevice").WithTags("Devices");
        }
    }
    
    public class GetDeviceQuery : IRequest<Response>
    {
        public int DeviceId { get; set; }
    }
    
    public class GetDeviceQueryHandler(MedTrackerDbContext context) : IRequestHandler<GetDeviceQuery, Response?>
    {
        public async Task<Response?> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
        { 
            return (await context.Devices.FindAsync([request.DeviceId], cancellationToken)) is { } device 
                ? new Response(device.Id, device.Description, device.Manufacturer, device.Model, device.PartNumber, device.LotNumber) : null;
        }
    }
}