using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.EducationHandlers;

public class PostEducationHandler : IRequestHandler<PostEducationCommand, EducationPutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostEducationHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<EducationPutDto> Handle(PostEducationCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Education education = _mapper.Map<Education>(request.EducationPostDto);
      _context.Educations.Add(education);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<EducationPutDto>(education);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}
