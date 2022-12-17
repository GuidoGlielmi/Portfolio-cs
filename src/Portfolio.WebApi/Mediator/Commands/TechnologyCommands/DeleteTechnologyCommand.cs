using MediatR;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

public record DeleteTechnologyCommand(Guid Id) : ICommand<Unit> { }

