using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.TechnologyValidators;

public class PostTechnologyCommandValidator : AbstractValidator<PostTechnologyCommand>
{
  public PostTechnologyCommandValidator()
  {
    RuleFor(e => e.TechnologyPostDto.Name)
      .MinimumLength(3)
      .MaximumLength(60);
    RuleFor(e => e.TechnologyPostDto.TechImg)
      .Matches("^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
  }
}