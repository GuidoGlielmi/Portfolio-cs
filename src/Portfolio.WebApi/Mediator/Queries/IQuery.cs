using MediatR;

namespace Portfolio.WebApi.Mediator.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}