using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;
using Portfolio.WebApi.Mediator.Queries.ExperienceQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class PatchExperienceHandler : IRequestHandler<PatchExperienceCommand>
{
  private readonly IMediator _mediator;

  public PatchExperienceHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<Unit> Handle(PatchExperienceCommand request, CancellationToken cancellationToken)
  {
    ExperiencePutDto foundExperience = await _mediator.Send(new GetExperienceByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundExperience);
    await _mediator.Send(new PutExperienceCommand(foundExperience), cancellationToken);
    return Unit.Value;
  }
}