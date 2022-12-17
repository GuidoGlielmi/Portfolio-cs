using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.SkillQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class GetSkillsHandler : IRequestHandler<GetSkillsQuery, IEnumerable<SkillPutDto>>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetSkillsHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<SkillPutDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var skills = (IEnumerable<Skill>)await _context.Skills.ToListAsync(cancellationToken);
      skills = skills.DynamicWhereAll(request.SearchObj);
      return _mapper.Map<IEnumerable<SkillPutDto>>(skills);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
