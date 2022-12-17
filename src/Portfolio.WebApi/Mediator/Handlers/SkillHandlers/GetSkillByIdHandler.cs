using MediatR;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Mediator.Queries.SkillQueries;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class GetSkillByIdHandler : IRequestHandler<GetSkillByIdQuery, SkillPutDto>
{
  private readonly IMediator _mediator;

  public GetSkillByIdHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<SkillPutDto> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
  {
    var skills = await _mediator.Send(new GetSkillsQuery(null), cancellationToken);
    return skills.FirstOrDefault(e => e.Id == request.Id);
  }
}

