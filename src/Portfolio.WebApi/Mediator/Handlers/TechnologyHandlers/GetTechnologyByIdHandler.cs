using MediatR;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Mediator.Queries.TechnologyQueries;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class GetTechnologyByIdHandler : IRequestHandler<GetTechnologyByIdQuery, TechnologyPutDto>
{
  private readonly IMediator _mediator;

  public GetTechnologyByIdHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task<TechnologyPutDto> Handle(GetTechnologyByIdQuery request, CancellationToken cancellationToken)
  {
    var technologies = await _mediator.Send(new GetTechnologiesQuery(null), cancellationToken);
    return technologies.FirstOrDefault(e => e.Id == request.Id);
  }
}
