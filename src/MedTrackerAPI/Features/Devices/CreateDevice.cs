using FluentValidation;
using MediatR;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Devices;

public static class CreateDevice
{
    public record Response (int DeviceId, string Description, string Manufacturer, string Model, string PartNumber, string LotNumber);
    
    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Manufacturer).NotEmpty().WithMessage("Manufacturer is required.");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.");
            RuleFor(x => x.PartNumber).NotEmpty().WithMessage("Part number is required.");
            RuleFor(x => x.LotNumber).NotEmpty().WithMessage("Lot number is required.");
        }
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("devices", async (IMediator mediator, CreateDeviceCommand command) =>
                {
                    var device = await mediator.Send(command);

                    return Results.CreatedAtRoute("GetDevice", new { device.DeviceId }, device);
                }).WithTags("Devices")
                .AddEndpointFilter<ValidationFilter<CreateDeviceCommand>>();
        }
    }
    
    public class CreateDeviceCommand() : IRequest<Response>
    {
        public string Description { get; set; }
        public string? SerialNumber { get; set; }
        public string  Manufacturer { get; set; }
        public string  Model { get; set; }
        public string  PartNumber { get; set; }
        public string  LotNumber { get; set; }
    }
    
    public class CreateDeviceCommandHandler(MedTrackerDbContext context) : IRequestHandler<CreateDeviceCommand, Response?>
    {
        public async Task<Response?> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = new Device
            {
                Description = request.Description,
                Manufacturer = request.Manufacturer,
                Model = request.Model,
                PartNumber = request.PartNumber,
                LotNumber = request.LotNumber,
                SerialNumber = request.SerialNumber
            };

            context.Devices.Add(device);
            await context.SaveChangesAsync(cancellationToken);

            return new Response(device.Id, device.Description, device.Manufacturer, device.Model, device.PartNumber, device.LotNumber);
        }
    }
}