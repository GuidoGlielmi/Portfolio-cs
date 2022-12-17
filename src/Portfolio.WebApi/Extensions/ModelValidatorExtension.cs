using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.Extensions;

public static class ModelValidatorExtension
{
  public static bool Validate(this object target, out IEnumerable<string> validationResults)
  {
    var validationContext = new ValidationContext(target);
    var results = new List<ValidationResult>();
    // validationResults must be initialized because, as opossed to "ref", "out" doesn't make sure of it.
    var isValid = Validator.TryValidateObject(target, validationContext, results, true);
    validationResults = results.Select(r => r.ErrorMessage);
    return isValid;
  }
}
