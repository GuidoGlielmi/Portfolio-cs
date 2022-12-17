using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class PutSkillHandler : IRequestHandler<PutSkillCommand>
{
  private readonly PortfolioContext _context;

  private readonly IMapper _mapper;

  public PutSkillHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Unit> Handle(PutSkillCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Skill skill = _mapper.Map<Skill>(request.SkillPutDto);
      _context.Entry(skill).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
