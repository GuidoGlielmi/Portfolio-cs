using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Mapper;
using System.Text.Json.Serialization;

namespace Portfolio.WebApi.Models;

public class Technology :
  IMapFrom<TechnologyPostDto>,
  IMapFrom<TechnologyPutDto>,
  IMapFrom<IEnumerable<TechnologyPostDto>>,
  IMapFrom<IEnumerable<TechnologyPutDto>>
{
  public Guid Id { get; set; }

  public string Name { get; set; }

  public string TechImg { get; set; }

  [JsonIgnore]
  public List<Project> Projects { get; set; } = new();

  [JsonIgnore]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
