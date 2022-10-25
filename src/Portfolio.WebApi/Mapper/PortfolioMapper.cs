using AutoMapper;

namespace Portfolio.WebApi.Mapper;

public class PortfolioMapper<T, TCreateDto, TPutDto>
  where T : class
  where TCreateDto : class
  where TPutDto : class
{
  private readonly IMapper Mapper;

  public PortfolioMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public TCreateDto ToPostDto(T entity) => Mapper.Map<TCreateDto>(entity);

  public IEnumerable<TCreateDto> ToPostDto(IEnumerable<T> entity) => entity.Select(e => Mapper.Map<TCreateDto>(e));


  public T FromPostDto(TCreateDto entity) => Mapper.Map<T>(entity);

  public IEnumerable<T> FromPostDto(IEnumerable<TCreateDto> entity) => entity.Select(e => Mapper.Map<T>(e));


  public TPutDto ToPutDto(T entity) => Mapper.Map<TPutDto>(entity);

  public IEnumerable<TPutDto> ToPutDto(IEnumerable<T> entity) => entity.Select(e => Mapper.Map<TPutDto>(e));


  public T FromPutDto(TPutDto entity) => Mapper.Map<T>(entity);

  public IEnumerable<T> FromPutDto(IEnumerable<TPutDto> entity) => entity.Select(e => Mapper.Map<T>(e));

}
