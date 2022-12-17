using MediatR;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Mediator.Queries.ProjectQueries;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectPutDto>
{
  private readonly IMediator _mediator;

  public GetProjectByIdHandler(IMediator mediator)
  {
    _mediator = mediator;
  }
  public async Task<ProjectPutDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
  {
    var projects = await _mediator.Send(new GetProjectsQuery(null), cancellationToken);
    return projects.FirstOrDefault(e => e.Id == request.Id);
  }
}
