using MediatR;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

public record DeleteExperienceCommand(Guid Id) : ICommand<Unit> { }
