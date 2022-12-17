using MediatR;
using Portfolio.WebApi.DTO.TechnologyDtos;

namespace Portfolio.WebApi.Mediator.Queries.TechnologyQueries;

public record GetTechnologyByIdQuery(Guid Id) : IRequest<TechnologyPutDto> { }
