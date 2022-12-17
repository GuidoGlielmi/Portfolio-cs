using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.EducationDtos;

namespace Portfolio.WebApi.Mediator.Commands.EducationCommands;
public record PatchEducationCommand(Guid Id, JsonPatchDocument<EducationPutDto> PatchDocument) : ICommand<Unit> { }
