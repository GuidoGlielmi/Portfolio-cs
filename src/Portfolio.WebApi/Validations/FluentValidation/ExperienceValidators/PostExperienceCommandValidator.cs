using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.ExperienceValidators;

public class PostExperienceCommandValidator : AbstractValidator<PostExperienceCommand>
{
  public PostExperienceCommandValidator()
  {
    RuleFor(e => e.ExperiencePostDto.EndDate)
      .Must(endDate =>
        DateTime.TryParse(endDate, out DateTime _)
        || endDate.ToLower() == "current")
      .WithMessage("Invalid EndDate");
    RuleFor(e => e.ExperiencePostDto.StartDate)
      .Must(startDate => DateTime.TryParse(startDate, out DateTime _))
      .WithMessage("Invalid StartDate");
    RuleFor(e => e.ExperiencePostDto.Description)
      .MinimumLength(20);
    RuleFor(e => e.ExperiencePostDto.Title)
      .MinimumLength(10);
    RuleFor(e => e.ExperiencePostDto.ExperienceImg)
      .Matches(@"^\./assets/logos/\w+\.[a-zA-Z]{2,5}$")
      .WithMessage("Incorrect image path");
  }
}