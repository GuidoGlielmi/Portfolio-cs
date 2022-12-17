using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.ExperienceDtos;

namespace Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

public record PatchExperienceCommand(Guid Id, JsonPatchDocument<ExperiencePutDto> PatchDocument) : ICommand<Unit> { }
