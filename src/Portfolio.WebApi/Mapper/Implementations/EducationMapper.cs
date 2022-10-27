using AutoMapper;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;

public class EducationMapper : IPortfolioMapper<Education, EducationPostDto, EducationPutDto>
{
  private readonly IMapper Mapper;

  public EducationMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public EducationPostDto ToPostDto(Education entity) => Mapper.Map<EducationPostDto>(entity);

  public IEnumerable<EducationPostDto> ToPostDto(IEnumerable<Education> entity) => entity.Select(e => Mapper.Map<EducationPostDto>(e));


  public Education FromPostDto(EducationPostDto entity) => Mapper.Map<Education>(entity);

  public IEnumerable<Education> FromPostDto(IEnumerable<EducationPostDto> entity) => entity.Select(e => Mapper.Map<Education>(e));


  public EducationPutDto ToPutDto(Education entity) => Mapper.Map<EducationPutDto>(entity);

  public IEnumerable<EducationPutDto> ToPutDto(IEnumerable<Education> entity) => entity.Select(e => Mapper.Map<EducationPutDto>(e));


  public Education FromPutDto(EducationPutDto entity) => Mapper.Map<Education>(entity);

  public IEnumerable<Education> FromPutDto(IEnumerable<EducationPutDto> entity) => entity.Select(e => Mapper.Map<Education>(e));

}
