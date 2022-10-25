using System.ComponentModel.DataAnnotations;
using static Portfolio.WebApi.Models.Skill;

namespace Portfolio.WebApi.Validations.CustomAttributes;

[AttributeUsage(AttributeTargets.Property |
  AttributeTargets.Field/*, AllowMultiple = false*/)]
sealed public class SkillTypeValidationAttribute : ValidationAttribute
{

  public SkillTypeValidationAttribute() => ErrorMessage = "Invalid skill type";

  public override bool IsValid(object value)
  {
    return Enum.TryParse(((string)value).ToUpper(), out SkillTypes _);
  }

  //public override string FormatErrorMessage(string name)
  //{
  //return String.Format(CultureInfo.CurrentCulture,
  //  ErrorMessageString, name);
  //}
}