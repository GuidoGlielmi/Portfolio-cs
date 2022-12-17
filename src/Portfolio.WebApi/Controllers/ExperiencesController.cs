using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO.Experience;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class ExperiencesController : ControllerBase
{
  private readonly IPortfolioService<Experience, ExperienceSearcheable> _repo;

  private readonly IPortfolioMapper<Experience, ExperiencePostDto, ExperiencePutDto> _mapper;


  public ExperiencesController(IPortfolioService<Experience, ExperienceSearcheable> service,
    PortfolioMapper<Experience, ExperiencePostDto, ExperiencePutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Experiences
  [HttpGet]
  [AllowAnonymous]
  public async Task<IEnumerable<ExperiencePutDto>> GetExperiences([FromQuery] ExperienceSearcheable searchObj)
  {
    IEnumerable<Experience> experiences = await _repo.GetAll();
    if (ModelState.Count > 0)
    {
      experiences = experiences.DynamicWhereAll(searchObj);
    }
    return _mapper.ToPutDto(experiences);
  }

  // GET: api/Experiences/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ExperiencePutDto>> GetExperience(Guid id)
  {
    Experience experience = await _repo.GetById(id);
    return _mapper.ToPutDto(experience);
  }

  // PUT: api/Experiences/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutExperience(ExperiencePutDto experienceDto)
  {
    Experience experience = _mapper.FromPutDto(experienceDto);
    await _repo.Update(experience);
    return Ok();
  }

  // POST: api/Experiences
  [HttpPost]
  public async Task<ActionResult<ExperiencePutDto>> PostExperience(ExperiencePostDto experienceDto)
  {
    Experience experience = _mapper.FromPostDto(experienceDto);
    await _repo.Create(experience);
    var response = _mapper.ToPutDto(experience);
    return CreatedAtAction(nameof(GetExperience), new { id = experience.Id }, response);
  }

  // DELETE: api/Experiences/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteExperience(Guid id)
  {
    Experience experience = await _repo.GetById(id);
    await _repo.Delete(experience);
    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchExperience(Guid id, JsonPatchDocument<Experience> patchDocument)
  {
    Experience foundExperience = await _repo.GetById(id);

    patchDocument.ApplyTo(foundExperience, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundExperience))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      //return StatusCode(400, new RequestException(400, errors).Error);
      return StatusCode(400, errors);
    }

    await _repo.Update(foundExperience);
    return Ok();
  }
}
