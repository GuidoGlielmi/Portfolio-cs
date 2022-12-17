using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Filters;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class TechnologiesController : ControllerBase
{
  private readonly ITechnologyService<Technology, TechnologySearcheable> _repo;

  private readonly IPortfolioMapper<Technology, TechnologyPostDto, TechnologyPutDto> _mapper;

  public TechnologiesController(ITechnologyService<Technology, TechnologySearcheable> repo,
    PortfolioMapper<Technology, TechnologyPostDto, TechnologyPutDto> mapper)
  {
    _repo = repo;
    _mapper = mapper;
  }

  // GET: api/Technologies
  [HttpGet]
  [AllowAnonymous]
  public async Task<IEnumerable<TechnologyPutDto>> GetTechnologies([FromQuery] TechnologySearcheable searchObj)
  {
    IEnumerable<Technology> technologies = await _repo.GetAll();
    if (ModelState.Count > 0)
    {
      technologies = technologies.DynamicWhereAll(searchObj);
    }
    return _mapper.ToPutDto(technologies);
    //return Ok(new Searcheable<TechnologyPutDto, TechnologySearcheable>(_mapper.ToPutDto(technologies).ToList(), searchObj));
  }

  // GET: api/Technologies/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<TechnologyPutDto>> GetTechnology(Guid id)
  {
    Technology technology = await _repo.GetById(id);
    return _mapper.ToPutDto(technology);
  }

  // PUT: api/Technologies/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutTechnology(TechnologyPutDto technologyDto)
  {
    Technology technology = _mapper.FromPutDto(technologyDto);
    await _repo.Update(technology);
    return Ok();
  }

  // POST: api/Technologies
  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<TechnologyPutDto>> PostTechnology(TechnologyPostDto technologyDto)
  {
    Technology technology = _mapper.FromPostDto(technologyDto);
    technology.Projects = await _repo.GetProjects(technologyDto.ProjectsIds);
    await _repo.Create(technology);
    var response = _mapper.ToPutDto(technology);
    return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, response);
  }

  // DELETE: api/Technologies/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteTechnology(Guid id)
  {
    Technology technology = await _repo.GetById(id);
    await _repo.Delete(technology);
    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> PatchTechnology(Guid id, JsonPatchDocument<Technology> patchDocument)
  {
    Technology foundTech = await _repo.GetById(id);

    patchDocument.ApplyTo(foundTech, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundTech))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      //return StatusCode(400, new RequestException(400, errors).Error);
      return StatusCode(400, errors);
    }

    await _repo.Update(foundTech);
    return Ok();
  }
}
