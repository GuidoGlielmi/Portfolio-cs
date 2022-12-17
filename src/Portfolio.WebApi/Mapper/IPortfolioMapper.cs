using AutoMapper;

namespace Portfolio.WebApi.Mapper;

public interface IPortfolioMapper<T, TPostDto, TPutDto>
 where T : class
 where TPostDto : class
 where TPutDto : class
{
  IMapper Mapper { get; }

  public TPostDto ToPostDto(T entity) => Mapper.Map<TPostDto>(entity);

  public IEnumerable<TPostDto> ToPostDto(IEnumerable<T> entity) => entity.Select(e => Mapper.Map<TPostDto>(e));


  public T FromPostDto(TPostDto entity) => Mapper.Map<T>(entity);

  public IEnumerable<T> FromPostDto(IEnumerable<TPostDto> entity) => entity.Select(e => Mapper.Map<T>(e));


  public TPutDto ToPutDto(T entity) => Mapper.Map<TPutDto>(entity);

  public IEnumerable<TPutDto> ToPutDto(IEnumerable<T> entity) => entity.Select(e => Mapper.Map<TPutDto>(e));


  public T FromPutDto(TPutDto entity) => Mapper.Map<T>(entity);

  public IEnumerable<T> FromPutDto(IEnumerable<TPutDto> entity) => entity.Select(e => Mapper.Map<T>(e));

}
