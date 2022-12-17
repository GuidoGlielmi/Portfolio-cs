using MediatR;
using Portfolio.WebApi.DTO.SkillDtos;

namespace Portfolio.WebApi.Mediator.Queries.SkillQueries;

public record GetSkillByIdQuery(Guid Id) : IRequest<SkillPutDto> { }