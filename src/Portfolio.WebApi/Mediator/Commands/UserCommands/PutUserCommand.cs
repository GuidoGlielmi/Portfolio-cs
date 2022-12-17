using MediatR;
using Portfolio.WebApi.DTO.UserDtos;

namespace Portfolio.WebApi.Mediator.Commands.UserCommands;

public record PutUserCommand(UserPutDto UserPutDto) : ICommand<Unit> { }