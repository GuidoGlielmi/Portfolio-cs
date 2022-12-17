using MediatR;
using Portfolio.WebApi.Mediator.Commands;

namespace Portfolio.WebApi.Mediator.Handlers;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
  where TCommand : ICommand<TResponse>
{
}
