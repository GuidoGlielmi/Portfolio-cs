using Portfolio.WebApi.DTO.ProjectUrlDtos;
using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.ProjectDtos;

public class ProjectPostDto
{
  [Required]
  [UrlRegexValidator(ErrorMessage = "Invalid deploy url")]
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
  public List<ProjectUrlPostDto> Urls { get; set; } = new();

  [MinLength(1, ErrorMessage = "Projects should have at least one technology")]
  public List<Guid> TechsIds { get; set; } = new();

  public Guid UserId { get; set; }
}


/*
{
  "deployUrl": "www.asd.com",
  "description": "stringstringstringst",
  "projectImg": "./assets/logos/asd.asd",
  "title": "string",
  "urls": [
    {
      "url": "www.asd.com",
      "name": "string"
    }
  ],
  "techs": [
    "b9f02685-1384-43ff-923a-165ba1c5bdaa"
  ],
  "userId": "a8fece7c-7037-4760-bbc1-b5cd6f456f69"
}
*/