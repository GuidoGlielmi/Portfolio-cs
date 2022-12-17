using MediatR;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.SkillCommands;

public record DeleteSkillCommand(Guid Id) : ICommand<Unit> { }

