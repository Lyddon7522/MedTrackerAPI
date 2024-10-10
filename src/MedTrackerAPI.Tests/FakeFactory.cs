using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Tests;

public static class FakeFactory
{
    public static Device CreateFakeDevice()
    {
        var autoFaker = new AutoFaker<Device>()
            .RuleFor(d => d.Id, f => f.Random.Int(min: 1, max: int.MaxValue))
            .Ignore(d => d.Supplies);
        
        return autoFaker.Generate();
    }

    public static Device WithSupply(this Device device, Action<Supply>? configure = null)
    {
        device.Supplies ??= [];
        
        var supplies = new AutoFaker<Supply>()
            .RuleFor(s => s.Id, f => f.Random.Int(min: 1, max: int.MaxValue))
            .RuleFor(s => s.DeviceId, _ => device.Id)
            .RuleFor(s => s.Device, f => device)
            .Generate();

        configure?.Invoke(supplies);

        device.Supplies.Add(supplies);

        return device;
    }
}