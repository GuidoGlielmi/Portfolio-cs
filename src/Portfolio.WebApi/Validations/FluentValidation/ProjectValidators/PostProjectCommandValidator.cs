using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.ProjectValidators;

public class PostProjectCommandValidator : AbstractValidator<PostProjectCommand>
{
  public PostProjectCommandValidator()
  {
    RuleFor(e => e.ProjectPostDto.DeployUrl)
      .Matches(@"^(https:\/\/)?(www\.)?\w{3,50}(\.[a-zA-Z]{2,5}){1,2}(\/\w+)*$");
    RuleFor(e => e.ProjectPostDto.Description)
      .MinimumLength(20);
    RuleFor(e => e.ProjectPostDto.ProjectImg)
      .Matches("^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
    RuleFor(e => e.ProjectPostDto.Title)
      .MinimumLength(5);
    RuleFor(e => e.ProjectPostDto.UserId)
      .NotEmpty();
  }
}