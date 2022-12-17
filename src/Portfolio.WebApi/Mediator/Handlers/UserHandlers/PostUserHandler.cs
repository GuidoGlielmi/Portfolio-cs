using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.UserCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.UserHandlers;

public class PostUserHandler : IRequestHandler<PostUserCommand, UserPutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostUserHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<UserPutDto> Handle(PostUserCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var user = _mapper.Map<User>(request.UserPostDto);
      _context.Users.Add(user);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<UserPutDto>(user);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}