using Portfolio.WebApi.DTO.ProjectDtos;

namespace Portfolio.WebApi.Mediator.Commands.ProjectCommands;

public record PostProjectCommand(ProjectPostDto ProjectPostDto) : ICommand<ProjectPutDto> { }

