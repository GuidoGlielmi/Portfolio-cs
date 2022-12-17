using MediatR;
using Portfolio.WebApi.DTO.EducationDtos;

namespace Portfolio.WebApi.Mediator.Queries.EducationQueries;

public record GetEducationByIdQuery(Guid Id) : IRequest<EducationPutDto> { }
