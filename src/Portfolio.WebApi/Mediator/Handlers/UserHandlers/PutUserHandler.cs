using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.UserCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class PutUserHandler : IRequestHandler<PutUserCommand>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PutUserHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Unit> Handle(PutUserCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var user = _mapper.Map<User>(request.UserPutDto);
      _context.Entry(user).State = EntityState.Modified;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
