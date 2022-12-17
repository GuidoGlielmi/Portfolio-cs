using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;
using Portfolio.WebApi.Mediator.Queries.ProjectQueries;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class PatchProjectHandler : IRequestHandler<PatchProjectCommand>
{
  private readonly IMediator _mediator;

  public PatchProjectHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task<Unit> Handle(PatchProjectCommand request, CancellationToken cancellationToken)
  {
    ProjectPutDto foundProject = await _mediator.Send(new GetProjectByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundProject);
    await _mediator.Send(new PutProjectCommand(foundProject), cancellationToken);
    return Unit.Value;
  }
}
