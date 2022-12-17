using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Queries.UserQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserPutDto>>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public GetUsersHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<IEnumerable<UserPutDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
  {
    try
    {
      var users = (IEnumerable<User>)await _context.Users.ToListAsync(cancellationToken);
      users = users.DynamicWhereAll(request.SearchObj);
      return _mapper.Map<IEnumerable<UserPutDto>>(users);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}