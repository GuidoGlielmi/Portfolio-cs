using FluentValidation;
using Newtonsoft.Json.Linq;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using static Portfolio.WebApi.Models.Skill;

namespace Portfolio.WebApi.Validations.FluentValidation.SkillValidators;

public class PutSkillCommandValidator : AbstractValidator<PutSkillCommand>
{
  public PutSkillCommandValidator()
  {
    RuleFor(s => s.SkillPutDto.Id).NotEmpty();
    RuleFor(s => s.SkillPutDto.AbilityPercentage)
      .LessThanOrEqualTo(100)
      .GreaterThanOrEqualTo(0);
    RuleFor(s => s.SkillPutDto.Name)
    .MinimumLength(5);
    RuleFor(s => s.SkillPutDto.Type)
      .Must(s => Enum.TryParse((s).ToUpper(), out SkillTypes _));
  }
}