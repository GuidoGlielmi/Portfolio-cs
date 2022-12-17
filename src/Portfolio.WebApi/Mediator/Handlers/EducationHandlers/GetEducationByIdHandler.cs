using MediatR;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class GetEducationByIdHandler : IRequestHandler<GetEducationByIdQuery, EducationPutDto>
{
  private readonly IMediator _mediator;
  private readonly ILogger<GetEducationByIdHandler> _logger;

  public GetEducationByIdHandler(IMediator mediator, ILogger<GetEducationByIdHandler> logger)
  {
    _mediator = mediator;
    _logger = logger;
  }
  public async Task<EducationPutDto> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
  {
    var educations = await _mediator.Send(new GetEducationsQuery(null), cancellationToken);
    var foundEd = educations.FirstOrDefault(e => e.Id == request.Id);
    if (foundEd == null)
    {
      _logger.LogCritical("The createdEducation with id:{id} was not found", request.Id);
      throw new RequestException(404);
    }
    return foundEd;
  }
}
