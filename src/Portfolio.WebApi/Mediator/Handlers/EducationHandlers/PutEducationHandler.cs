using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class PutEducationHandler : IRequestHandler<PutEducationCommand>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PutEducationHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Unit> Handle(PutEducationCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var education = _mapper.Map<Education>(request.EducationPutDto);
      _context.Entry(education).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
