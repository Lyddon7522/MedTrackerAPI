using FluentValidation;
using MediatR;
using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Features.Devices;

public static class CreateDevice
{
    private record Response (int DeviceId);
    
    /*public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Manufacturer).NotEmpty();
            RuleFor(x => x.Model).NotEmpty();
            RuleFor(x => x.PartNumber).NotEmpty();
            RuleFor(x => x.LotNumber).NotEmpty();
        }
    }*/
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("devices", async (IMediator mediator, CreateDeviceCommand command) =>
            {
               var deviceId = await mediator.Send(command);
               
               return Results.Ok(new Response(deviceId));
            }).WithTags("Devices");
        }
    }
    
    public class CreateDeviceCommand() : IRequest<int>
    {
        public string Description { get; set; }
        public string? SerialNumber { get; set; }
        public string  Manufacturer { get; set; }
        public string  Model { get; set; }
        public string  PartNumber { get; set; }
        public string  LotNumber { get; set; }
    }
    
    public class CreateDeviceCommandHandler(MedTrackerDbContext context) : IRequestHandler<CreateDeviceCommand, int>
    {
        public async Task<int> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            /*var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }*/
            
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
            
            return device.Id;
        }
    }
}