using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.ProjectUrlDtos;

public class ProjectUrlPutDto
{
  public Guid Id { get; set; }

  [Required]
  [UrlRegexValidator(ErrorMessage = "Invalid project url")]
  public string Url { get; set; }

  [Required]
  [MinLength(5)]
  public string Name { get; set; }

  public Guid ProjectId { get; set; }
}
