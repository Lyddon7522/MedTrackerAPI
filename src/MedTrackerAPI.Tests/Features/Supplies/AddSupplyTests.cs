using FluentValidation.TestHelper;
using MedTrackerAPI.Features.Supplies;
using MedTrackerAPI.Infrastructure;
using static MedTrackerAPI.Tests.FakeFactory;

namespace MedTrackerAPI.Tests.Features.Supplies;

public class AddSupplyTests
{
    private Device _device;

    [SetUp]
    public void Setup()
    {
        _device = CreateFakeDevice();
    }
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoDescription_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = value
        }).ShouldHaveValidationErrorFor(x => x.Description);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPartNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            PartNumber = value
        }).ShouldHaveValidationErrorFor(x => x.PartNumber);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoLotNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            LotNumber = value
        }).ShouldHaveValidationErrorFor(x => x.LotNumber);
    
    [TestCase(0)]
    public void GivenNoDeviceId_ThenIsNotValid(int value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            DeviceId = value
        }).ShouldHaveValidationErrorFor(x => x.DeviceId);
    
    [Test]
    public void GivenAllValidFields_ThenIsValid()
    {
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = _device.WithSupplies().Description,
            Manufacturer = _device.WithSupplies().Manufacturer,
            PartNumber = _device.WithSupplies().PartNumber,
            LotNumber = _device.WithSupplies().LotNumber,
            DeviceId = _device.Id
        }).ShouldNotHaveAnyValidationErrors();
    }
}