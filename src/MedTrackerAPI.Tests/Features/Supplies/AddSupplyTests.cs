using FluentValidation.TestHelper;
using MedTrackerAPI.Features.Supplies;
using MedTrackerAPI.Infrastructure;
using static MedTrackerAPI.Tests.FakeFactory;

namespace MedTrackerAPI.Tests.Features.Supplies;

public class AddSupplyTests
{
    private Device _device;
    private Supply _supply;

    [SetUp]
    public void Setup()
    {
        _device = CreateFakeDevice();
        _supply = CreateFakeSuppliesForDevice(_device.Id).First();
    }
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoDescription_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = value,
            PartNumber = _supply.PartNumber,
            LotNumber = _supply.LotNumber,
            DeviceId = _supply.DeviceId
        }).ShouldHaveValidationErrorFor(x => x.Description);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPartNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = _supply.Description,
            PartNumber = value,
            LotNumber = _supply.LotNumber,
            DeviceId = _supply.DeviceId
        }).ShouldHaveValidationErrorFor(x => x.PartNumber);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoLotNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = _supply.Description,
            PartNumber = _supply.PartNumber,
            LotNumber = value,
            DeviceId = _supply.DeviceId
        }).ShouldHaveValidationErrorFor(x => x.LotNumber);
    
    [TestCase(0)]
    public void GivenNoDeviceId_ThenIsNotValid(int value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = _supply.Description,
            PartNumber = _supply.PartNumber,
            LotNumber = _supply.LotNumber,
            DeviceId = value
        }).ShouldHaveValidationErrorFor(x => x.DeviceId);
    
    [Test]
    public void GivenAllValidFields_ThenIsValid()
    {
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = _supply.Description,
            Manufacturer = _supply.Manufacturer,
            PartNumber = _supply.PartNumber,
            LotNumber = _supply.LotNumber,
            DeviceId = _supply.DeviceId
        }).ShouldNotHaveAnyValidationErrors();
    }
}