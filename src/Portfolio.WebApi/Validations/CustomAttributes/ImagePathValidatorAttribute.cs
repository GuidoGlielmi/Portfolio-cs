using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Portfolio.WebApi.Validations.CustomAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ImagePathValidatorAttribute : ValidationAttribute
{

  public ImagePathValidatorAttribute() => ErrorMessage = "Invalid image path";

  public override bool IsValid(object value)
  {
    var pathRegex = new Regex(@"^\./assets/logos/\w+\.[a-zA-Z]{2,5}$");
    return pathRegex.IsMatch((string)value);
  }
}
