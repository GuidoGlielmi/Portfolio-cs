using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.UserCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
  private readonly PortfolioContext _context;

  public DeleteUserHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var user = await _context.Users.FindAsync(request.Id, cancellationToken);
      _context.Users.Remove(user);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
