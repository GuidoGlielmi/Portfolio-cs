using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.Validations.CustomAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MonthYearDateValidationAttribute : ValidationAttribute
{

  public MonthYearDateValidationAttribute() => ErrorMessage = "Invalid start date";

  public override bool IsValid(object value)
  {
    return DateTime.TryParse((string)value, out DateTime _);
  }
}
