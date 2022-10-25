using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.TechnologyDtos;

public class TechnologyInProjectDto
{
  public Guid Id { get; set; }

  [MaxLength(60)]
  [MinLength(3)]
  [Required]
  public string Name { get; set; }

  [Required]
  [ImagePathValidator(ErrorMessage = "Invalid technology image path")]
  public string TechImg { get; set; }
}