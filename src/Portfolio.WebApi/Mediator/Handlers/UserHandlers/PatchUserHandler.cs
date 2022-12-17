using MediatR;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Mediator.Commands.UserCommands;
using Portfolio.WebApi.Mediator.Queries.UserQueries;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class PatchUserHandler : IRequestHandler<PatchUserCommand>
{
  private readonly IMediator _mediator;

  public PatchUserHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<Unit> Handle(PatchUserCommand request, CancellationToken cancellationToken)
  {
    UserPutDto foundUser = await _mediator.Send(new GetUserByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundUser);
    await _mediator.Send(new PutUserCommand(foundUser), cancellationToken);
    return Unit.Value;
  }
}