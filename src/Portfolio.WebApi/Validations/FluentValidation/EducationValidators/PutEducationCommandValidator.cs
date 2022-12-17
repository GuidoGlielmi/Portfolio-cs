using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.EducationValidators;

public class PutEducationCommandValidator : AbstractValidator<PutEducationCommand>
{
  // called after CreateEducationCommand
  public PutEducationCommandValidator()
  {
    RuleFor(e => e.EducationPutDto.Id).NotEmpty();
    RuleFor(e => e.EducationPutDto.EndDate)
      .Must(endDate =>
        DateTime.TryParse(endDate, out DateTime _)
        || endDate.ToLower() == "current")
      .WithMessage("Invalid EndDate");
    RuleFor(e => e.EducationPutDto.StartDate)
      .Must(startDate => DateTime.TryParse(startDate, out DateTime _))
      .WithMessage("Invalid StartDate");
    RuleFor(e => e.EducationPutDto.Degree)
      .MinimumLength(10);
    RuleFor(e => e.EducationPutDto.School)
      .MinimumLength(10);
    RuleFor(e => e.EducationPutDto.EducationImg)
      .Matches(@"^\./assets/logos/\w+\.[a-zA-Z]{2,5}$")
      .WithMessage("Incorrect image path");
  }
  //protected override bool PreValidate(ValidationContext<PutEducationCommand> context, ValidationResult result)
  //{ 
  //}
}