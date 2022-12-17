using MediatR;
using Portfolio.WebApi.DTO.ProjectDtos;

namespace Portfolio.WebApi.Mediator.Commands.ProjectCommands;

public record PutProjectCommand(ProjectPutDto ProjectPutDto) : ICommand<Unit> { }