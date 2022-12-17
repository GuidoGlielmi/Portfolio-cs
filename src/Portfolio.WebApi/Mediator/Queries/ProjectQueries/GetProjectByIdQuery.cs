using MediatR;
using Portfolio.WebApi.DTO.ProjectDtos;

namespace Portfolio.WebApi.Mediator.Queries.ProjectQueries;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectPutDto> { }
