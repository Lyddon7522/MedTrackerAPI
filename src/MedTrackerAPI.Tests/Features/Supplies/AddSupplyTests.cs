using FluentValidation.TestHelper;
using MedTrackerAPI.Infrastructure;
using static MedTrackerAPI.Features.Supplies.AddSupply;
using static MedTrackerAPI.Tests.FakeFactory;

namespace MedTrackerAPI.Tests.Features.Supplies;

public class AddSupplyTests
{
    private Device _device = null!;
    private Supply _supply = null!;
    
    [SetUp]
    public void Setup()
    {
        _device = CreateFakeDevice().WithSupply();
        _supply = _device.Supplies.First();
    }
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoDescription_ThenIsNotValid(string? value) =>
        new AddSupplyCommandValidator().TestValidate(new AddSupplyCommand
        {
            Description = value
        }).ShouldHaveValidationErrorFor(x => x.Description);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPartNumber_ThenIsNotValid(string? value) =>
        new AddSupplyCommandValidator().TestValidate(new AddSupplyCommand
        {
            PartNumber = value
        }).ShouldHaveValidationErrorFor(x => x.PartNumber);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoLotNumber_ThenIsNotValid(string? value) =>
        new AddSupplyCommandValidator().TestValidate(new AddSupplyCommand
        {
            LotNumber = value
        }).ShouldHaveValidationErrorFor(x => x.LotNumber);
    
    [TestCase(0)]
    public void GivenNoDeviceId_ThenIsNotValid(int value) =>
        new AddSupplyCommandValidator().TestValidate(new AddSupplyCommand
        {
            DeviceId = value
        }).ShouldHaveValidationErrorFor(x => x.DeviceId);
    
    [Test]
    public void GivenAllValidFields_ThenIsValid() => 
        new AddSupplyCommandValidator().TestValidate(new AddSupplyCommand
        {
            Description = _supply.Description,
            Manufacturer = _supply.Manufacturer,
            PartNumber = _supply.PartNumber,
            LotNumber = _supply.LotNumber,
            DeviceId = _device.Id
        }).ShouldNotHaveAnyValidationErrors();
}