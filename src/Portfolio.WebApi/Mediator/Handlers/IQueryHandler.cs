using MediatR;
using Portfolio.WebApi.Mediator.Queries;

namespace Portfolio.WebApi.Mediator.Handlers;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
  where TQuery : IQuery<TResponse>
{
}