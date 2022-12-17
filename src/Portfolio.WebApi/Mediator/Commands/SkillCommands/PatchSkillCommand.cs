using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.SkillDtos;

namespace Portfolio.WebApi.Mediator.Commands.SkillCommands;

public record PatchSkillCommand(Guid Id, JsonPatchDocument<SkillPutDto> PatchDocument) : ICommand<Unit> { }
