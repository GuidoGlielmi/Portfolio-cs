using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Portfolio.WebApi.Validations.CustomAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class UrlRegexValidatorAttribute : ValidationAttribute
{
  public UrlRegexValidatorAttribute() => ErrorMessage = "Invalid url";

  public override bool IsValid(object value)
  {
    var pathRegex = new Regex(@"^(https:\/\/)?(www\.)?\w{3,50}(\.[a-zA-Z]{2,5}){1,2}(\/\w+)*$");
    return pathRegex.IsMatch((string)value);
  }
}
