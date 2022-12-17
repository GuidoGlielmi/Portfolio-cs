using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.ProjectDtos;

namespace Portfolio.WebApi.Mediator.Commands.ProjectCommands;

public record PatchProjectCommand(Guid Id, JsonPatchDocument<ProjectPutDto> PatchDocument) : ICommand<Unit> { }

