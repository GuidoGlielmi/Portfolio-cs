using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.SkillDtos;

public class SkillPutDto
{
  public Guid Id { get; set; }

  [Required]
  [Range(0, 100)]
  public int AbilityPercentage { get; set; }

  [Required]
  [MinLength(5)]
  public string Name { get; set; }

  [Required]
  [SkillTypeValidation]
  public string Type { get; set; }
}
