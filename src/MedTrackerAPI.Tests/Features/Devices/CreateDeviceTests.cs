using FluentValidation.TestHelper;
using MedTrackerAPI.Features.Devices;

namespace MedTrackerAPI.Tests.Features.Devices;

public class CreateDeviceTests
{
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoDescription_ThenIsNotValid(string? value) =>
        new CreateDevice.CreateDeviceCommandValidator().TestValidate(new CreateDevice.CreateDeviceCommand
        {
            Description = value
        }).ShouldHaveValidationErrorFor(x => x.Description);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoManufacturer_ThenIsNotValid(string? value) =>
        new CreateDevice.CreateDeviceCommandValidator()
            .TestValidate(new CreateDevice.CreateDeviceCommand
            {
                Manufacturer = value
            }).ShouldHaveValidationErrorFor(x => x.Manufacturer);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoModel_ThenIsNotValid(string? value) =>
        new CreateDevice.CreateDeviceCommandValidator()
            .TestValidate(new CreateDevice.CreateDeviceCommand
            {
                Model = value
            }).ShouldHaveValidationErrorFor(x => x.Model);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPartNumber_ThenIsNotValid(string? value) =>
        new CreateDevice.CreateDeviceCommandValidator()
            .TestValidate(new CreateDevice.CreateDeviceCommand
            {
                PartNumber = value
            }).ShouldHaveValidationErrorFor(x => x.PartNumber);
    
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoLotNumber_ThenIsNotValid(string? value) =>
        new CreateDevice.CreateDeviceCommandValidator()
            .TestValidate(new CreateDevice.CreateDeviceCommand
            {
                LotNumber = value
            }).ShouldHaveValidationErrorFor(x => x.LotNumber);

    [Test]
    public void GivenAllFields_ThenIsValid() =>
        new CreateDevice.CreateDeviceCommandValidator().TestValidate(new CreateDevice.CreateDeviceCommand
        {
            Description = "Test",
            Manufacturer = "Test Manufacturer",
            Model = "Test Model",
            PartNumber = "12345678",
            LotNumber = "F260"
        }).ShouldNotHaveAnyValidationErrors();



}
