using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Mediator.Queries.ExperienceQueries;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class GetExperienceByIdHandler : IRequestHandler<GetExperienceByIdQuery, ExperiencePutDto>
{
  private readonly IMediator _mediator;

  public GetExperienceByIdHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<ExperiencePutDto> Handle(GetExperienceByIdQuery request, CancellationToken cancellationToken)
  {
    var experiences = await _mediator.Send(new GetExperiencesQuery(null), cancellationToken);
    return experiences.FirstOrDefault(e => e.Id == request.Id);
  }
}
