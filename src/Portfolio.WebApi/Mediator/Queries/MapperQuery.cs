using MediatR;

namespace Portfolio.WebApi.Mediator.Queries;

public class MapperQuery<TFrom, TTo> : IRequest<IEnumerable<TTo>>
{
}
