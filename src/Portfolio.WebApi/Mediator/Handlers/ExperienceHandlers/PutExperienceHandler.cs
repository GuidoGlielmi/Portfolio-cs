using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class PutExperienceHandler : IRequestHandler<PutExperienceCommand>
{
  private readonly PortfolioContext _context;

  private readonly IMapper _mapper;

  public PutExperienceHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Unit> Handle(PutExperienceCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Experience experience = _mapper.Map<Experience>(request.ExperiencePutDto);
      _context.Entry(experience).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
