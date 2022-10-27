using AutoMapper;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;

public class ProjectMapper : IPortfolioMapper<Project, ProjectPostDto, ProjectPutDto>
{
  private readonly IMapper Mapper;

  public ProjectMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public ProjectPostDto ToPostDto(Project entity) => Mapper.Map<ProjectPostDto>(entity);

  public IEnumerable<ProjectPostDto> ToPostDto(IEnumerable<Project> entity) => entity.Select(e => Mapper.Map<ProjectPostDto>(e));


  public Project FromPostDto(ProjectPostDto entity) => Mapper.Map<Project>(entity);

  public IEnumerable<Project> FromPostDto(IEnumerable<ProjectPostDto> entity) => entity.Select(e => Mapper.Map<Project>(e));


  public ProjectPutDto ToPutDto(Project entity) => Mapper.Map<ProjectPutDto>(entity);

  public IEnumerable<ProjectPutDto> ToPutDto(IEnumerable<Project> entity) => entity.Select(e => Mapper.Map<ProjectPutDto>(e));


  public Project FromPutDto(ProjectPutDto entity) => Mapper.Map<Project>(entity);

  public IEnumerable<Project> FromPutDto(IEnumerable<ProjectPutDto> entity) => entity.Select(e => Mapper.Map<Project>(e));

}
