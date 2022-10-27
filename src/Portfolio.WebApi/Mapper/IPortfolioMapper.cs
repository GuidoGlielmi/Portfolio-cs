namespace Portfolio.WebApi.Mapper;

public interface IPortfolioMapper<T, TCreateDto, TPutDto>
  where T : class
  where TCreateDto : class
  where TPutDto : class
{
  public TCreateDto ToPostDto(T entity);

  public IEnumerable<TCreateDto> ToPostDto(IEnumerable<T> entity);


  public T FromPostDto(TCreateDto entity);

  public IEnumerable<T> FromPostDto(IEnumerable<TCreateDto> entity);


  public TPutDto ToPutDto(T entity);

  public IEnumerable<TPutDto> ToPutDto(IEnumerable<T> entity);


  public T FromPutDto(TPutDto entity);

  public IEnumerable<T> FromPutDto(IEnumerable<TPutDto> entity);

}
