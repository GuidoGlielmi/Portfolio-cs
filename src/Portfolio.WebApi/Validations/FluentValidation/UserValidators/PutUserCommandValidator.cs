using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.UserCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.UserValidators;

public class PutUserCommandValidator : AbstractValidator<PutUserCommand>
{
  public PutUserCommandValidator()
  {
    RuleFor(e => e.UserPutDto.Id).NotEmpty();
    RuleFor(e => e.UserPutDto.Username)
      .MinimumLength(20)
      .MaximumLength(300);
    RuleFor(e => e.UserPutDto.Password)
      .MinimumLength(5)
      .MaximumLength(50);
    RuleFor(e => e.UserPutDto.GithubUrl)
      .Matches(@"^(https:\/\/)?(www\.)?github\.com\/\w+$");
    RuleFor(e => e.UserPutDto.LinkedInUrl)
      .Matches(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/\w+$");
    RuleFor(e => e.UserPutDto.ProfileImg)
      .Matches(@"^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
    RuleFor(e => e.UserPutDto.AboutMe)
      .MinimumLength(20)
      .MaximumLength(300);
    RuleFor(e => e.UserPutDto.FirstName).NotEmpty();
  }
}