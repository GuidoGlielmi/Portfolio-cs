using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.ExperienceValidators;

public class PutExperienceCommandValidator : AbstractValidator<PutExperienceCommand>
{
  public PutExperienceCommandValidator()
  {
    RuleFor(e => e.ExperiencePutDto.Id).NotEmpty();
    RuleFor(e => e.ExperiencePutDto.EndDate)
      .Must(endDate =>
        DateTime.TryParse(endDate, out DateTime _)
        || endDate.ToLower() == "current")
      .WithMessage("Invalid EndDate");
    RuleFor(e => e.ExperiencePutDto.StartDate)
      .Must(startDate => DateTime.TryParse(startDate, out DateTime _))
      .WithMessage("Invalid StartDate");
    RuleFor(e => e.ExperiencePutDto.Description)
      .MinimumLength(20);
    RuleFor(e => e.ExperiencePutDto.Title)
      .MinimumLength(10);
    RuleFor(e => e.ExperiencePutDto.ExperienceImg)
      .Matches(@"^\./assets/logos/\w+\.[a-zA-Z]{2,5}$")
      .WithMessage("Incorrect image path");
  }
}