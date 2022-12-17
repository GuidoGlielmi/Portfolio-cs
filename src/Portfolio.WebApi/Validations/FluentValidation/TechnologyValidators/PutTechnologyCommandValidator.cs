using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.TechnologyValidators;

public class PutTechnologyCommandValidator : AbstractValidator<PutTechnologyCommand>
{
  public PutTechnologyCommandValidator()
  {
    RuleFor(e => e.TechnologyPutDto.Id).NotEmpty();
    RuleFor(e => e.TechnologyPutDto.Name)
      .MinimumLength(3)
      .MaximumLength(60);
    RuleFor(e => e.TechnologyPutDto.TechImg)
      .Matches("^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
  }
}