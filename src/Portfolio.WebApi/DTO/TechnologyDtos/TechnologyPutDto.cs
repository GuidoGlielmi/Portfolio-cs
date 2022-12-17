
namespace Portfolio.WebApi.DTO.TechnologyDtos;

public class TechnologyPutDto
{
  public Guid Id { get; set; }

  //[MaxLength(60)]
  //[MinLength(3)]
  //[Required]
  public string Name { get; set; }

  //[Required]
  //[ImagePathValidator]
  public string TechImg { get; set; }

  //public List<ProjectPutDto> Projects { get; set; } = new();
}