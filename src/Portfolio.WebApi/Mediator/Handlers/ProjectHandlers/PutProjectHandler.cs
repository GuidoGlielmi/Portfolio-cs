using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class PutProjectHandler : IRequestHandler<PutProjectCommand>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PutProjectHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<Unit> Handle(PutProjectCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Project project = _mapper.Map<Project>(request.ProjectPutDto);
      _context.Entry(project).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
