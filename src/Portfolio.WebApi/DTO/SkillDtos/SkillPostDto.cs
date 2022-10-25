using Microsoft.Extensions.Primitives;
using Portfolio.WebApi.Validations.CustomAttributes;
using Portfolio.WebApi.Errors;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static Portfolio.WebApi.Models.Skill;

namespace Portfolio.WebApi.DTO.SkillDtos;

public class SkillPostDto
{
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
