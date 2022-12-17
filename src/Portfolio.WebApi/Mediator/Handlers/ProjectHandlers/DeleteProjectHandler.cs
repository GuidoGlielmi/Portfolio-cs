using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
{
  private readonly PortfolioContext _context;

  public DeleteProjectHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var project = await _context.Projects.FindAsync(request.Id, cancellationToken);
      _context.Projects.Remove(project);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
