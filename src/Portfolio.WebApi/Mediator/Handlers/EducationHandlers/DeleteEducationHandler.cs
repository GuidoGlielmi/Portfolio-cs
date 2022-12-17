using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class DeleteEducationHandler : IRequestHandler<DeleteEducationCommand>
{
  private readonly PortfolioContext _context;

  public DeleteEducationHandler(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<Unit> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var education = await _context.Educations.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
      if (education == null)
      {
        throw new RequestException(404, "Education not found");
      }
      _context.Educations.Remove(education);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
