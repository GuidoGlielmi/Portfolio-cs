using MediatR;

namespace Portfolio.WebApi.Mediator.Commands.EducationCommands;

public record DeleteEducationCommand(Guid Id) : ICommand<Unit> { }