using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using static Portfolio.WebApi.Models.Skill;

namespace Portfolio.WebApi.Validations.FluentValidation.SkillValidators;

public class PostSkillCommandValidator : AbstractValidator<PostSkillCommand>
{
  public PostSkillCommandValidator()
  {
    RuleFor(s => s.SkillPostDto.AbilityPercentage)
      .LessThanOrEqualTo(100)
      .GreaterThanOrEqualTo(0);
    RuleFor(s => s.SkillPostDto.Name)
    .MinimumLength(5);
    RuleFor(s => s.SkillPostDto.Type)
      .Must(s => Enum.TryParse((s).ToUpper(), out SkillTypes _));
  }
}