using Portfolio.WebApi.Validations.CustomAttributes;
using Portfolio.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.ExperienceDtos;

public class ExperiencePostDto
{
  [Required]
  [MinLength(10)]
  public string Title { get; set; }

  [RegularExpression(@"^\./assets/img/certificates/[a-zA-Z]+\.[a-zA-Z]{2,5}$", ErrorMessage = "Invalid experience certificate path")]
  public string Certificate { get; set; }

  [Required]
  [MinLength(10)]
  public string Description { get; set; }

  [Required]
  [ImagePathValidator(ErrorMessage = "Invalid education image path")]
  public string ExperienceImg { get; set; }

  [Required]
  [MonthYearDateValidation]
  public string StartDate { get; set; }

  [EndDateValidation]
  public string EndDate { get; private set; } = "Current";

  public Guid UserId { get; set; }
}
