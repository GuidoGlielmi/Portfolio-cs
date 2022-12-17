using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class DeleteExperienceHandler : IRequestHandler<DeleteExperienceCommand>
{
  private readonly PortfolioContext _context;

  public DeleteExperienceHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var experience = await _context.Experiences.FindAsync(request.Id, cancellationToken);
      _context.Experiences.Remove(experience);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
