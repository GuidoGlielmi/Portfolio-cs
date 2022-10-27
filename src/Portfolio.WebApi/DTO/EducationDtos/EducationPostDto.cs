using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;
using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.EducationDtos;

public class EducationPostDto : IMapFrom<Education>
{

  [Required]
  [MinLength(10)]
  public string Degree { get; set; }

  // ./assets/logos/asd.asd
  [Required]
  [ImagePathValidator(ErrorMessage = "Invalid education image path")]
  public string EducationImg { get; set; }

  [Required]
  [MinLength(10)]
  public string School { get; set; }

  [Required]
  [MonthYearDateValidation]
  public string StartDate { get; set; }

  [EndDateValidation]
  public string EndDate { get; set; } = "Current";

  public Guid UserId { get; set; } // an education can be created even without a User type prop
  // An UserId is sufficient

}
