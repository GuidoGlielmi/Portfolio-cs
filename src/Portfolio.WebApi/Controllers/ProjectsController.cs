using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Filters;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class ProjectsController : ControllerBase
{
  private readonly IProjectService<Project, ProjectSearcheable> _repo;

  private readonly IPortfolioMapper<Project, ProjectPostDto, ProjectPutDto> _mapper;

  public ProjectsController(IProjectService<Project, ProjectSearcheable> service,
    PortfolioMapper<Project, ProjectPostDto, ProjectPutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Projects
  [HttpGet]
  [AllowAnonymous]
  public async Task<IEnumerable<ProjectPutDto>> GetProjects([FromQuery] ProjectSearcheable searchObj)
  {
    IEnumerable<Project> projects = await _repo.GetAll();
    if (ModelState.Count > 0)
    {
      projects = projects.DynamicWhereAll(searchObj);
    }
    return _mapper.ToPutDto(projects);
    //return Ok(new Searcheable<ProjectPutDto, ProjectSearcheable>(_mapper.ToPutDto(projects).ToList(), searchObj));
  }

  // GET: api/Projects/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ProjectPutDto> GetProject(Guid id)
  {
    Project project = await _repo.GetById(id);
    return _mapper.ToPutDto(project);
  }

  // PUT: api/Projects/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutProject(ProjectPutDto projectDto)
  {
    Project project = _mapper.FromPutDto(projectDto);
    await _repo.Update(project);
    return Ok();
  }

  // POST: api/Projects
  [HttpPost]
  public async Task<ActionResult<ProjectPutDto>> PostProject(ProjectPostDto projectDto)
  {
    Project project = _mapper.FromPostDto(projectDto);
    project.Techs = await _repo.GetTechs(projectDto.TechsIds);
    await _repo.Create(project);
    var response = _mapper.ToPutDto(project);
    return CreatedAtAction(nameof(GetProject), new { id = project.Id }, response);
  }

  // DELETE: api/Projects/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProject(Guid id)
  {
    Project project = await _repo.GetById(id);
    await _repo.Delete(project);
    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchProject(Guid id, JsonPatchDocument<Project> patchDocument)
  {
    Project foundProject = await _repo.GetById(id);

    patchDocument.ApplyTo(foundProject, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundProject))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      //return StatusCode(400, new RequestException(400, errors).Error);
      return StatusCode(400, errors);
    }

    await _repo.Update(foundProject);
    return Ok();
  }
}
