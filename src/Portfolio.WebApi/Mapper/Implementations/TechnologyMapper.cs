using AutoMapper;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;
public class TechnologyMapper : IPortfolioMapper<Technology, TechnologyPostDto, TechnologyPutDto>
{
  private readonly IMapper Mapper;

  public TechnologyMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public TechnologyPostDto ToPostDto(Technology entity) => Mapper.Map<TechnologyPostDto>(entity);

  public IEnumerable<TechnologyPostDto> ToPostDto(IEnumerable<Technology> entity) => entity.Select(e => Mapper.Map<TechnologyPostDto>(e));


  public Technology FromPostDto(TechnologyPostDto entity) => Mapper.Map<Technology>(entity);

  public IEnumerable<Technology> FromPostDto(IEnumerable<TechnologyPostDto> entity) => entity.Select(e => Mapper.Map<Technology>(e));


  public TechnologyPutDto ToPutDto(Technology entity) => Mapper.Map<TechnologyPutDto>(entity);

  public IEnumerable<TechnologyPutDto> ToPutDto(IEnumerable<Technology> entity) => entity.Select(e => Mapper.Map<TechnologyPutDto>(e));


  public Technology FromPutDto(TechnologyPutDto entity) => Mapper.Map<Technology>(entity);

  public IEnumerable<Technology> FromPutDto(IEnumerable<TechnologyPutDto> entity) => entity.Select(e => Mapper.Map<Technology>(e));

}
