using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.ProjectValidators;

public class PutProjectCommandValidator : AbstractValidator<PutProjectCommand>
{
  public PutProjectCommandValidator()
  {
    RuleFor(e => e.ProjectPutDto.Id).NotEmpty();
    RuleFor(e => e.ProjectPutDto.DeployUrl)
      .Matches(@"^(https:\/\/)?(www\.)?\w{3,50}(\.[a-zA-Z]{2,5}){1,2}(\/\w+)*$");
    RuleFor(e => e.ProjectPutDto.Description)
      .MinimumLength(20);
    RuleFor(e => e.ProjectPutDto.ProjectImg)
      .Matches("^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
    RuleFor(e => e.ProjectPutDto.Title)
      .MinimumLength(5);
    RuleFor(e => e.ProjectPutDto.UserId)
      .NotEmpty();
  }
}