using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.TechnologyCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.TechnologyHandlers;

public class PostTechnologyHandler : IRequestHandler<PostTechnologyCommand, TechnologyPutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostTechnologyHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<TechnologyPutDto> Handle(PostTechnologyCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var tech = _mapper.Map<Technology>(request.TechnologyPostDto);
      _context.Technologies.Add(tech);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<TechnologyPutDto>(tech);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}