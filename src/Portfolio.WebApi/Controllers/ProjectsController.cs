using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Errors;
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

  private readonly PortfolioMapper<Project, ProjectPostDto, ProjectPutDto> _mapper;

  public ProjectsController(IProjectService<Project, ProjectSearcheable> service, PortfolioMapper<Project, ProjectPostDto, ProjectPutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Projects
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<ProjectPutDto>>> GetProjects([FromQuery] ProjectSearcheable searchObj)
  {
    try
    {
      IEnumerable<Project> projects = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        projects = _repo.Filter(projects, searchObj);
      }
      return new ResponseDto<ProjectPutDto>(_mapper.ToPutDto(projects));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Projects/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<ProjectPutDto>>> GetProject(Guid id)
  {
    try
    {
      Project project = await _repo.GetById(id);
      return new ResponseDto<ProjectPutDto>(_mapper.ToPutDto(project));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Projects/5
  [HttpPut("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PutProject(ProjectPutDto projectDto)
  {
    try
    {
      Project project = _mapper.FromPutDto(projectDto);
      await _repo.Update(project);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Projects
  [HttpPost]
  public async Task<ActionResult<ResponseDto<ProjectPutDto>>> PostProject(ProjectPostDto projectDto)
  {
    try
    {
      Project project = _mapper.FromPostDto(projectDto);
      project.Techs = await _repo.GetTechs(projectDto.TechsIds);
      await _repo.Create(project);
      var response = new ResponseDto<ProjectPutDto>(_mapper.ToPutDto(project));
      return CreatedAtAction(nameof(GetProject), new { id = project.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Projects/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteProject(Guid id)
  {
    try
    {
      Project project = await _repo.GetById(id);
      await _repo.Delete(project);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchProject(Guid id, JsonPatchDocument<Project> patchDocument)
  {
    Project foundProject;
    try
    {
      foundProject = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundProject, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundProject))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    try
    {
      await _repo.Update(foundProject);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

}
