using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;

namespace Portfolio.WebApi.Mediator.Queries.ExperienceQueries;

public record GetExperienceByIdQuery(Guid Id) : IRequest<ExperiencePutDto> { }