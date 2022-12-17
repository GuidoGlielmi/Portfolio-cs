using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class DeleteTechnologyHandler : IRequestHandler<DeleteTechnologyCommand>
{
  private readonly PortfolioContext _context;

  public DeleteTechnologyHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteTechnologyCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var tech = await _context.Technologies.FindAsync(request.Id, cancellationToken);
      _context.Technologies.Remove(tech);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
