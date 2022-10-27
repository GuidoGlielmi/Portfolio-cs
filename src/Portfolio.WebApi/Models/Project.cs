
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Mapper;

namespace Portfolio.WebApi.Models;

public class Project : UserResource,
  IMapFrom<ProjectPostDto>,
  IMapFrom<ProjectPutDto>,
  IMapFrom<IEnumerable<ProjectPostDto>>,
  IMapFrom<IEnumerable<ProjectPutDto>>
{
  public Guid Id { get; set; }

  public string Title { get; set; }

  public string DeployUrl { get; set; }

  public string Description { get; set; }

  public string ProjectImg { get; set; }

  public List<ProjectUrl> Urls { get; set; } = new();

  public List<Technology> Techs { get; set; } = new();
}
