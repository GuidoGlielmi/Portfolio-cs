using AutoMapper;
using MediatR;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands.ProjectCommands;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Handlers.ProjectHandlers;

public class PostProjectHandler : IRequestHandler<PostProjectCommand, ProjectPutDto>
{
  private readonly PortfolioContext _context;
  private readonly IMapper _mapper;

  public PostProjectHandler(PortfolioContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
  public async Task<ProjectPutDto> Handle(PostProjectCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var project = _mapper.Map<Project>(request.ProjectPostDto);
      _context.Projects.Add(project);
      await _context.SaveChangesAsync(cancellationToken);
      return _mapper.Map<ProjectPutDto>(project);
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }
}