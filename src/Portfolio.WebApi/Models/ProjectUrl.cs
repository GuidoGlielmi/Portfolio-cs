using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.Models;

public class ProjectUrl //: IValidatableObject
{
  public Guid Id { get; set; }

  [Required]
  [RegularExpression(@"^(https:\/\/)?(www\.)?\w{3,50}(\.[a-zA-Z]{2,5}){1,2}(\/\w+)*$", ErrorMessage = "Invalid url")]
  public string Url { get; set; }

  [Required]
  public string Name { get; set; }

  public Project Project { get; set; } = new();

  public Guid ProjectId { get; set; }

}
/*
  //[CustomValidation(typeof(ProjectUrl), nameof(IsValidUrl))]
  public static ValidationResult IsValidUrl(object url, ValidationContext pValidationContext)
  {
    //the argument type will be that of the element where the annotation was used (property or class).
    if (Uri.IsWellFormedUriString((string)url, UriKind.RelativeOrAbsolute))
    {
      return ValidationResult.Success;
    }
    return new ValidationResult("Invalid URL");
  } 
 */