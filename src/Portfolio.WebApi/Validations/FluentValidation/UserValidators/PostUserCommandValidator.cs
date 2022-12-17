using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.UserCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.UserValidators;

public class PostUserCommandValidator : AbstractValidator<PostUserCommand>
{
  public PostUserCommandValidator()
  {
    RuleFor(e => e.UserPostDto.Username)
      .MinimumLength(20)
      .MaximumLength(300);
    RuleFor(e => e.UserPostDto.Password)
      .MinimumLength(5)
      .MaximumLength(50);
    RuleFor(e => e.UserPostDto.GithubUrl)
      .Matches(@"^(https:\/\/)?(www\.)?github\.com\/\w+$");
    RuleFor(e => e.UserPostDto.LinkedInUrl)
      .Matches(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/\w+$");
    RuleFor(e => e.UserPostDto.ProfileImg)
      .Matches(@"^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$");
    RuleFor(e => e.UserPostDto.AboutMe)
      .MinimumLength(20)
      .MaximumLength(300);
    RuleFor(e => e.UserPostDto.FirstName).NotEmpty();
  }
}