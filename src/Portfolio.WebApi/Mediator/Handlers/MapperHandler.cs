using AutoMapper;
using MediatR;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Queries;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers;

public class MapperHandler<TFrom, TTo> : IRequestHandler<MapperQuery<TFrom, TTo>, IEnumerable<TTo>>
{
  private readonly IMapper _mapper;
  public MapperHandler(IMapper mapper)
  {
    _mapper = mapper;
  }
  public Task<IEnumerable<TTo>> Handle(MapperQuery<TFrom, TTo> request, CancellationToken cancellationToken)
  {

    throw new NotImplementedException();
  }
}
