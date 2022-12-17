using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Mapper;

namespace Portfolio.WebApi.Models;

public class Experience : UserResource,
  IMapFrom<ExperiencePostDto>,
  IMapFrom<ExperiencePutDto>,
  IMapFrom<IEnumerable<ExperiencePostDto>>,
  IMapFrom<IEnumerable<ExperiencePutDto>>
{
  public Guid Id { get; set; }

  public string Title { get; set; }

  public string Certificate { get; set; }

  public string Description { get; set; }

  public string EndDate { get; set; } = "Current";

  public string StartDate { get; set; }

  public string ExperienceImg { get; set; }
}
