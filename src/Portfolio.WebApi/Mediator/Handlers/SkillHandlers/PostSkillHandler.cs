using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.SkillCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.SkillHandlers;

public class PostSkillHandler : IRequestHandler<PostSkillCommand, SkillPutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostSkillHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<SkillPutDto> Handle(PostSkillCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var skill = _mapper.Map<Skill>(request.SkillPostDto);
      _context.Skills.Add(skill);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<SkillPutDto>(skill);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}