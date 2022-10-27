using AutoMapper;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;

public class ExperienceMapper : IPortfolioMapper<Experience, ExperiencePostDto, ExperiencePutDto>
{
  private readonly IMapper Mapper;

  public ExperienceMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public ExperiencePostDto ToPostDto(Experience entity) => Mapper.Map<ExperiencePostDto>(entity);

  public IEnumerable<ExperiencePostDto> ToPostDto(IEnumerable<Experience> entity) => entity.Select(e => Mapper.Map<ExperiencePostDto>(e));


  public Experience FromPostDto(ExperiencePostDto entity) => Mapper.Map<Experience>(entity);

  public IEnumerable<Experience> FromPostDto(IEnumerable<ExperiencePostDto> entity) => entity.Select(e => Mapper.Map<Experience>(e));


  public ExperiencePutDto ToPutDto(Experience entity) => Mapper.Map<ExperiencePutDto>(entity);

  public IEnumerable<ExperiencePutDto> ToPutDto(IEnumerable<Experience> entity) => entity.Select(e => Mapper.Map<ExperiencePutDto>(e));


  public Experience FromPutDto(ExperiencePutDto entity) => Mapper.Map<Experience>(entity);

  public IEnumerable<Experience> FromPutDto(IEnumerable<ExperiencePutDto> entity) => entity.Select(e => Mapper.Map<Experience>(e));

}
