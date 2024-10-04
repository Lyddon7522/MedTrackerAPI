using MediatR;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrackerAPI.Features.Devices;

public static class GetDevice
{
    public record Response(int DeviceId, string Description, string Manufacturer, string Model, string PartNumber, string LotNumber);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("devices/{deviceId}", async (IMediator mediator, int deviceId) =>
                {
                    var query = new GetDeviceQuery { DeviceId = deviceId };
                    var device = await mediator.Send(query);
                    
                    return device is null ? Results.BadRequest("Device not found") : Results.Ok(device);
                }).WithTags("Devices");
        }
    }
    
    public class GetDeviceQuery : IRequest<Response>
    {
        public int DeviceId { get; set; }
    }
    
    public class GetDeviceQueryHandler(MedTrackerDbContext context) : IRequestHandler<GetDeviceQuery, Response>
    {
        public async Task<Response> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
        { 
            var device = await context.Devices.Where(d => d.Id == request.DeviceId).SingleOrDefaultAsync(cancellationToken);
   
            if (device is null)
            {
                return null!;
            }
            return new Response(device.Id, device.Description, device.Manufacturer, device.Model, device.PartNumber, device.LotNumber);
        }
    }
}