using MediatR;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Mediator.Queries.UserQueries;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserPutDto>
{
  private readonly IMediator _mediator;

  public GetUserByIdHandler(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task<UserPutDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var users = await _mediator.Send(new GetUsersQuery(null), cancellationToken);
    return users.FirstOrDefault(e => e.Id == request.Id);
  }
}

