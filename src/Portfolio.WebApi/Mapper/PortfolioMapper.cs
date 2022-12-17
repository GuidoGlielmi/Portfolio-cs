using AutoMapper;

namespace Portfolio.WebApi.Mapper;

public class PortfolioMapper<T, TPostDto, TPutDto> : IPortfolioMapper<T, TPostDto, TPutDto>
  where T : class
  where TPostDto : class
  where TPutDto : class
{

  public IMapper Mapper { get; }

  public PortfolioMapper(IMapper mapper)
  {
    Mapper = mapper;
  }

}
