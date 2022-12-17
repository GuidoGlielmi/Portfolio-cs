using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class PutTechnologyHandler : IRequestHandler<PutTechnologyCommand>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PutTechnologyHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Unit> Handle(PutTechnologyCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Technology tech = _mapper.Map<Technology>(request.TechnologyPutDto);
      _context.Entry(tech).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
