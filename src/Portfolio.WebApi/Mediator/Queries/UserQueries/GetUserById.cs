using MediatR;
using Portfolio.WebApi.DTO.UserDtos;

namespace Portfolio.WebApi.Mediator.Queries.UserQueries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserPutDto> { }
