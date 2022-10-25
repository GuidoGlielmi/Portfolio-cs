using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Errors;
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

  private readonly PortfolioMapper<Technology, TechnologyPostDto, TechnologyPutDto> _mapper;

  public TechnologiesController(ITechnologyService<Technology,
    TechnologySearcheable> repo,
    PortfolioMapper<Technology, TechnologyPostDto, TechnologyPutDto> mapper)
  {
    _repo = repo;
    _mapper = mapper;
  }

  // GET: api/Technologies
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<TechnologyPutDto>>> GetTechnologies([FromQuery] TechnologySearcheable searchObj)
  {
    try
    {
      IEnumerable<Technology> technologies = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        technologies = _repo.Filter(technologies, searchObj);
      }
      return new ResponseDto<TechnologyPutDto>(_mapper.ToPutDto(technologies));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Technologies/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<TechnologyPutDto>>> GetTechnology(Guid id)
  {
    try
    {
      Technology technology = await _repo.GetById(id);
      return new ResponseDto<TechnologyPutDto>(_mapper.ToPutDto(technology));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Technologies/5
  [HttpPut("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PutTechnology(TechnologyPutDto technologyDto)
  {
    try
    {
      Technology technology = _mapper.FromPutDto(technologyDto);
      await _repo.Update(technology);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Technologies
  [HttpPost]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<TechnologyPutDto>>> PostTechnology(TechnologyPostDto technologyDto)
  {
    try
    {
      Technology technology = _mapper.FromPostDto(technologyDto);
      technology.Projects = await _repo.GetProjects(technologyDto.ProjectsIds);
      await _repo.Create(technology);
      var response = new ResponseDto<TechnologyPutDto>(_mapper.ToPutDto(technology));
      return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Technologies/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteTechnology(Guid id)
  {
    try
    {
      Technology technology = await _repo.GetById(id);
      await _repo.Delete(technology);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchTechnology(Guid id, JsonPatchDocument<Technology> patchDocument)
  {
    Technology foundTech;
    try
    {
      foundTech = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundTech, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundTech))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    try
    {
      await _repo.Update(foundTech);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }
}
