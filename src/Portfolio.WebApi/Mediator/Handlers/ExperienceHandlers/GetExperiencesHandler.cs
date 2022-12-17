using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.ExperienceQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class GetExperiencesHandler : IRequestHandler<GetExperiencesQuery, IEnumerable<ExperiencePutDto>>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetExperiencesHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<ExperiencePutDto>> Handle(GetExperiencesQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var experiences = (IEnumerable<Experience>)await _context.Experiences.ToListAsync(cancellationToken);
      experiences = experiences.DynamicWhereAll(request.SearchObj);
      return _mapper.Map<IEnumerable<ExperiencePutDto>>(experiences);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}