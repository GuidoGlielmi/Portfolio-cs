using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class DeleteSkillHandler : IRequestHandler<DeleteSkillCommand>
{
  private readonly PortfolioContext _context;

  public DeleteSkillHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var skill = await _context.Skills.FindAsync(request.Id, cancellationToken);
      _context.Skills.Remove(skill);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
