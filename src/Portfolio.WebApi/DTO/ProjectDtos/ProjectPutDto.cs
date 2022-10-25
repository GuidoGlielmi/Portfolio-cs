using Portfolio.WebApi.Validations.CustomAttributes;
using Portfolio.WebApi.DTO.ProjectUrlDtos;
using Portfolio.WebApi.DTO.TechnologyDtos;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.ProjectDtos;

public class ProjectPutDto
{
  public Guid Id { get; set; }

  [Required]
  [UrlRegexValidator(ErrorMessage = "Invalid project deploy url")]
  public string DeployUrl { get; set; }

  [Required]
  [MinLength(20)]
  public string Description { get; set; }

  [Required]
  [ImagePathValidator]
  public string ProjectImg { get; set; }

  [Required]
  [MinLength(5)]
  public string Title { get; set; }

  [MinLength(1, ErrorMessage = "Projects should have at least one URL")]
  public List<ProjectUrlPutDto> Urls { get; set; } = new();

  [MinLength(1, ErrorMessage = "Projects should have at least one technology")]
  public List<TechnologyPutDto> Techs { get; set; } = new();
  //public List<TechnologyInProjectDto> Techs { get; set; } = new();

  public Guid UserId { get; set; }
}
