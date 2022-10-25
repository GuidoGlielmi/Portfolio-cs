using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.EducationDtos;
public class EducationPutDto
{
  public Guid Id { get; set; }

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

  // [DataType(DataType.DateTime)] is used for html inputs
  // DateTime is a struct, structs are "value type", not "reference type", so their default value is not null
  // 0001-01-01T00:00:29
  [Required]
  [MonthYearDateValidation]
  public string StartDate { get; set; }

  [EndDateValidation]
  public string EndDate { get; set; } = "Current";

  public Guid UserId { get; set; } // an education can be created even without a User type prop
  // An UserId is sufficient
}
