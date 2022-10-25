using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.Experience;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class ExperiencesController : ControllerBase
{
  private readonly IService<Experience, ExperienceSearcheable> _repo;

  private readonly PortfolioMapper<Experience, ExperiencePostDto, ExperiencePutDto> _mapper;

  public ExperiencesController(IService<Experience, ExperienceSearcheable> service, PortfolioMapper<Experience, ExperiencePostDto, ExperiencePutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Experiences
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<ExperiencePutDto>>> GetExperiences([FromQuery] ExperienceSearcheable searchObj)
  {
    try
    {
      IEnumerable<Experience> experiences = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        experiences = _repo.Filter(experiences, searchObj);
      }
      return new ResponseDto<ExperiencePutDto>(_mapper.ToPutDto(experiences));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Experiences/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<ExperiencePutDto>>> GetExperience(Guid id)
  {
    try
    {
      Experience experience = await _repo.GetById(id);
      return new ResponseDto<ExperiencePutDto>(_mapper.ToPutDto(experience));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Experiences/5
  [HttpPut("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PutExperience(ExperiencePutDto experienceDto)
  {
    try
    {
      Experience experience = _mapper.FromPutDto(experienceDto);
      await _repo.Update(experience);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Experiences
  [HttpPost]
  public async Task<ActionResult<ResponseDto<ExperiencePutDto>>> PostExperience(ExperiencePostDto experienceDto)
  {
    try
    {
      Experience experience = _mapper.FromPostDto(experienceDto);
      await _repo.Create(experience);
      var response = new ResponseDto<ExperiencePutDto>(_mapper.ToPutDto(experience));
      return CreatedAtAction(nameof(GetExperience), new { id = experience.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Experiences/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteExperience(Guid id)
  {
    try
    {
      Experience experience = await _repo.GetById(id);
      await _repo.Delete(experience);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchExperience(Guid id, JsonPatchDocument<Experience> patchDocument)
  {
    Experience foundExperience;
    try
    {
      foundExperience = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundExperience, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundExperience))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    try
    {
      await _repo.Update(foundExperience);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }
}
