using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Errors;

using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class SkillsController : ControllerBase
{
  private readonly IService<Skill, SkillSearcheable> _repo;

  private readonly PortfolioMapper<Skill, SkillPostDto, SkillPutDto> _mapper;


  public SkillsController(IService<Skill, SkillSearcheable> service,
    PortfolioMapper<Skill, SkillPostDto, SkillPutDto> mapper)
  {
    _repo = service;
    _mapper = mapper;
  }

  // GET: api/Skills
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<SkillPutDto>>> GetSkills([FromQuery] SkillSearcheable searchObj)
  {
    try
    {
      IEnumerable<Skill> skills = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        skills = _repo.Filter(skills, searchObj);
      }
      return new ResponseDto<SkillPutDto>(_mapper.ToPutDto(skills));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Skills/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<SkillPutDto>>> GetSkill(Guid id)
  {
    try
    {
      Skill skill = await _repo.GetById(id);
      return new ResponseDto<SkillPutDto>(_mapper.ToPutDto(skill));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Skills/5
  [HttpPut("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PutSkill(SkillPutDto skillDto)
  {
    try
    {
      Skill skill = _mapper.FromPutDto(skillDto);
      await _repo.Update(skill);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Skills
  [HttpPost]
  public async Task<ActionResult<ResponseDto<SkillPutDto>>> PostSkill(SkillPostDto skillDto)
  {
    try
    {
      Skill skill = _mapper.FromPostDto(skillDto);
      await _repo.Create(skill);
      var response = new ResponseDto<SkillPutDto>(_mapper.ToPutDto(skill));
      return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Skills/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteSkill(Guid id)
  {
    try
    {
      Skill skill = await _repo.GetById(id);
      await _repo.Delete(skill);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchSkill(Guid id, JsonPatchDocument<Skill> patchDocument)
  {
    Skill foundSkill;
    try
    {
      foundSkill = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundSkill, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundSkill))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    try
    {
      await _repo.Update(foundSkill);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }
}