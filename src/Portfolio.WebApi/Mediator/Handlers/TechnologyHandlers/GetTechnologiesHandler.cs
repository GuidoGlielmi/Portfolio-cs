using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.TechnologyQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class GetTechnologiesHandler : IRequestHandler<GetTechnologiesQuery, IEnumerable<TechnologyPutDto>>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetTechnologiesHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<TechnologyPutDto>> Handle(GetTechnologiesQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var technologies = (IEnumerable<Technology>)await _context.Technologies.ToListAsync(cancellationToken);
      technologies = technologies.DynamicWhereAll(request.SearchObj);
      return _mapper.Map<IEnumerable<TechnologyPutDto>>(technologies);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}