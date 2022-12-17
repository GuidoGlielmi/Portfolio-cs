using MediatR;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;
using Portfolio.WebApi.Mediator.Queries.TechnologyQueries;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class PatchTechnologyHandler : IRequestHandler<PatchTechnologyCommand>
{
  private readonly IMediator _mediator;

  public PatchTechnologyHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<Unit> Handle(PatchTechnologyCommand request, CancellationToken cancellationToken)
  {
    TechnologyPutDto foundTechnology = await _mediator.Send(new GetTechnologyByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundTechnology);
    await _mediator.Send(new PutTechnologyCommand(foundTechnology), cancellationToken);
    return Unit.Value;
  }
}