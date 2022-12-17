using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.ProjectQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectPutDto>>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetProjectsHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<ProjectPutDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var projects = (IEnumerable<Project>)await _context.Projects.ToListAsync(cancellationToken);
      projects = projects.DynamicWhereAll(request.SearchObj);
      return _mapper.Map<IEnumerable<ProjectPutDto>>(projects);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}