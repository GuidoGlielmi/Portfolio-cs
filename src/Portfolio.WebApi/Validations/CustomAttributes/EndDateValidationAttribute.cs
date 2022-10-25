namespace Portfolio.WebApi.Validations.CustomAttributes;

public class EndDateValidationAttribute : MonthYearDateValidationAttribute
{

  public EndDateValidationAttribute() => ErrorMessage = "Invalid end date";

  public override bool IsValid(object value)
  {
    bool isValidDate = base.IsValid(value);
    bool isValidEndDate = isValidDate || ((string)value).ToLower() == "current";
    return isValidEndDate;
  }
}
