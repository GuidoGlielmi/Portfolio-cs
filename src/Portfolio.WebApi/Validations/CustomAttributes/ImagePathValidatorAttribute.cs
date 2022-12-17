using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Portfolio.WebApi.Validations.CustomAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public partial class ImagePathValidatorAttribute : ValidationAttribute
{

  public ImagePathValidatorAttribute() => ErrorMessage = "Invalid image path";

  public override bool IsValid(object value)
  {
    var pathRegex = MyRegex();
    return pathRegex.IsMatch((string)value);
  }

  [GeneratedRegex("^\\./assets/logos/\\w+\\.[a-zA-Z]{2,5}$")]
  private static partial Regex MyRegex();
}
