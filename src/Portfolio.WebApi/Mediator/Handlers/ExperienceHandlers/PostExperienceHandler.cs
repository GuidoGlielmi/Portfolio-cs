using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ExperienceCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ExperienceHandlers;

public class PostExperienceHandler : IRequestHandler<PostExperienceCommand, ExperiencePutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostExperienceHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<ExperiencePutDto> Handle(PostExperienceCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Experience experience = _mapper.Map<Experience>(request.ExperiencePostDto);
      _context.Experiences.Add(experience);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<ExperiencePutDto>(experience);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}