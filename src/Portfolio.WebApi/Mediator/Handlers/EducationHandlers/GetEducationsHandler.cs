using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class GetEducationsHandler : IRequestHandler<GetEducationsQuery, IEnumerable<EducationPutDto>>
// first generic argument must be of type IRequest, like any query or command should.
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetEducationsHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<IEnumerable<EducationPutDto>> Handle(GetEducationsQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var educations = (IEnumerable<Education>)await _context.Educations.AsNoTracking().ToListAsync(cancellationToken);
      if (request.SearchObj != null)
      {
        educations = educations.DynamicWhereAll(request.SearchObj);
      }
      return _mapper.Map<IEnumerable<EducationPutDto>>(educations);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
