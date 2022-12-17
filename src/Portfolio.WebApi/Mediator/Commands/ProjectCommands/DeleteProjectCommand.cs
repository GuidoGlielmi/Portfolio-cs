using MediatR;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.ProjectCommands;

public record DeleteProjectCommand(Guid Id) : ICommand<Unit> { }
