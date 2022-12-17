using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Filters;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class SkillsController : ControllerBase
{
  private readonly IPortfolioService<Skill, SkillSearcheable> _repo;

  private readonly IPortfolioMapper<Skill, SkillPostDto, SkillPutDto> _mapper;


  public SkillsController(IPortfolioService<Skill, SkillSearcheable> service,
    PortfolioMapper<Skill, SkillPostDto, SkillPutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Skills
  [HttpGet]
  [AllowAnonymous]
  public async Task<IEnumerable<SkillPutDto>> GetSkills([FromQuery] SkillSearcheable searchObj)
  {
    IEnumerable<Skill> skills = await _repo.GetAll();
    if (ModelState.Count > 0)
    {
      skills = skills.DynamicWhereAll(searchObj);
    }
    return _mapper.ToPutDto(skills);
  }

  // GET: api/Skills/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<SkillPutDto> GetSkill(Guid id)
  {
    Skill skill = await _repo.GetById(id);
    return _mapper.ToPutDto(skill);
  }

  // PUT: api/Skills/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutSkill(SkillPutDto skillDto)
  {
    Skill skill = _mapper.FromPutDto(skillDto);
    await _repo.Update(skill);
    return Ok();
  }

  // POST: api/Skills
  [HttpPost]
  public async Task<ActionResult<SkillPutDto>> PostSkill(SkillPostDto skillDto)
  {
    Skill skill = _mapper.FromPostDto(skillDto);
    await _repo.Create(skill);
    var response = _mapper.ToPutDto(skill);
    return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, response);
  }

  // DELETE: api/Skills/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteSkill(Guid id)
  {
    Skill skill = await _repo.GetById(id);
    await _repo.Delete(skill);
    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchSkill(Guid id, JsonPatchDocument<Skill> patchDocument)
  {
    Skill foundSkill = await _repo.GetById(id);

    patchDocument.ApplyTo(foundSkill, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundSkill))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      //return StatusCode(400, new RequestException(400, errors).Error);
      return StatusCode(400, errors);
    }

    await _repo.Update(foundSkill);
    return Ok();
  }
}