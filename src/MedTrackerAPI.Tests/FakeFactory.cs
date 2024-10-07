using MedTrackerAPI.Infrastructure;

namespace MedTrackerAPI.Tests;

public static class FakeFactory
{
    public static Device CreateFakeDevice()
    {
        var faker = new Faker<Device>()
            .RuleFor(d => d.Id, f => f.Random.Int(1, 1000))
            .RuleFor(d => d.Description, f => f.Commerce.ProductName())
            .RuleFor(d => d.SerialNumber, f => f.Commerce.Ean8())
            .RuleFor(d => d.Manufacturer, f => f.Company.CompanyName())
            .RuleFor(d => d.Model, f => f.Commerce.ProductMaterial())
            .RuleFor(d => d.PartNumber, f => f.Commerce.Ean8())
            .RuleFor(d => d.LotNumber, f => f.Random.AlphaNumeric(5))
            .RuleFor(d => d.Supplies, (f, d) => CreateFakeSuppliesForDevice(d.Id));;

        return faker.Generate();
    }

    public static List<Supply> CreateFakeSuppliesForDevice(int deviceId)
    {
        var faker = new Faker<Supply>()
            .RuleFor(s => s.Id, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.Description, f => f.Commerce.ProductName())
            .RuleFor(s => s.Manufacturer, f => f.Company.CompanyName())
            .RuleFor(s => s.PartNumber, f => f.Commerce.Ean8())
            .RuleFor(s => s.LotNumber, f => f.Random.AlphaNumeric(5))
            .RuleFor(s => s.DeviceId, f => deviceId);

        return faker.Generate(5);
    }
}