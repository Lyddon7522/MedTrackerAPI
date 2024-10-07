using FluentValidation.TestHelper;
using MedTrackerAPI.Features.Supplies;

namespace MedTrackerAPI.Tests.Features.Supplies;

public class AddSupplyTests
{
    [TestCase(null)]
    [TestCase("")]
    public void GivenNoDescription_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = value,
            PartNumber = "ValidPartNumber",
            LotNumber = "ValidLotNumber",
            DeviceId = 1
        }).ShouldHaveValidationErrorFor(x => x.Description);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoPartNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = "ValidDescription",
            PartNumber = value,
            LotNumber = "ValidLotNumber",
            DeviceId = 1
        }).ShouldHaveValidationErrorFor(x => x.PartNumber);

    [TestCase(null)]
    [TestCase("")]
    public void GivenNoLotNumber_ThenIsNotValid(string? value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = "ValidDescription",
            PartNumber = "ValidPartNumber",
            LotNumber = value,
            DeviceId = 1
        }).ShouldHaveValidationErrorFor(x => x.LotNumber);
    
    [TestCase(0)]
    public void GivenNoDeviceId_ThenIsNotValid(int value) =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = "ValidDescription",
            PartNumber = "ValidPartNumber",
            LotNumber = "ValidLotNumber",
            DeviceId = value
        }).ShouldHaveValidationErrorFor(x => x.DeviceId);
    
    [Test]
    public void GivenAllValidFields_ThenIsValid() =>
        new AddSupply.AddSupplyCommandValidator().TestValidate(new AddSupply.AddSupplyCommand
        {
            Description = "ValidDescription",
            PartNumber = "ValidPartNumber",
            LotNumber = "ValidLotNumber",
            DeviceId = 1
        }).ShouldNotHaveAnyValidationErrors();
}