using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.TechnologyDtos;

namespace Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

public record PatchTechnologyCommand(Guid Id, JsonPatchDocument<TechnologyPutDto> PatchDocument) : ICommand<Unit> { }

