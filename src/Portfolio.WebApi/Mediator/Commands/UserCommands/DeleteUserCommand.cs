using MediatR;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.UserCommands;

public record DeleteUserCommand(Guid Id) : ICommand<Unit> { }

