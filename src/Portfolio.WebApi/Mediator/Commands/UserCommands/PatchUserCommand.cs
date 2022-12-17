using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Portfolio.WebApi.DTO.UserDtos;

namespace Portfolio.WebApi.Mediator.Commands.UserCommands;

public record PatchUserCommand(Guid Id, JsonPatchDocument<UserPutDto> PatchDocument) : ICommand<Unit> { }
