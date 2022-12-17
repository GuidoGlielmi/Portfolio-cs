using MediatR;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using Portfolio.WebApi.Mediator.Queries.SkillQueries;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class PatchSkillHandler : IRequestHandler<PatchSkillCommand>
{
  private readonly IMediator _mediator;

  public PatchSkillHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<Unit> Handle(PatchSkillCommand request, CancellationToken cancellationToken)
  {
    SkillPutDto foundSkill = await _mediator.Send(new GetSkillByIdQuery(request.Id), cancellationToken);
    request.PatchDocument.ApplyTo(foundSkill);
    await _mediator.Send(new PutSkillCommand(foundSkill), cancellationToken);
    return Unit.Value;
  }
}