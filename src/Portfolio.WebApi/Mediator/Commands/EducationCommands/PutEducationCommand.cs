using MediatR;
using Portfolio.WebApi.DTO.EducationDtos;

namespace Portfolio.WebApi.Mediator.Commands.EducationCommands;

public record PutEducationCommand(EducationPutDto EducationPutDto) : ICommand<Unit> { }