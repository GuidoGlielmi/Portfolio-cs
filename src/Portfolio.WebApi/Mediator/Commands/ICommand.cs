using MediatR;

namespace Portfolio.WebApi.Mediator.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}