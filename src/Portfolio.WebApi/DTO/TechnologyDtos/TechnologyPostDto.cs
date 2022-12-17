
namespace Portfolio.WebApi.DTO.TechnologyDtos;

public class TechnologyPostDto
{

  //[MaxLength(60)]
  //[MinLength(3)]
  //[Required]
  public string Name { get; set; }

  //[Required]
  //[ImagePathValidator]
  public string TechImg { get; set; }

  public List<Guid> ProjectsIds { get; set; } = new();
}
