using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class SkillRepo : IService<Skill, SkillSearcheable>
{
  private readonly PortfolioContext _context;

  public SkillRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Skill>> GetAll()
  {
    try
    {
      return await _context.Skills.ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public IEnumerable<Skill> Filter(IEnumerable<Skill> skills, SkillSearcheable searchObj)
  {
    if (!string.IsNullOrEmpty(searchObj.Name))
    {
      skills = skills.Where(p => p.Name.ToLower() == searchObj.Name.ToLower());
    }
    if (searchObj.AbilityPercentage != null)
    {
      skills = skills.Where(p => p.AbilityPercentage == searchObj.AbilityPercentage);
    }
    if (!string.IsNullOrEmpty(searchObj.Type))
    {
      skills = skills.Where(p => p.Type.ToString().ToLower() == searchObj.Type.ToLower());
    }

    return skills;
  }

  public async Task<Skill> GetById(Guid id)
  {
    Skill foundSkill = await _context.Skills.FindAsync(id);
    return foundSkill ?? throw new RequestException(404);
  }

  public async Task Create(Skill skill)
  {
    try
    {
      _context.Skills.Add(skill);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Skill skill)
  {
    try
    {
      _context.Skills.Remove(skill);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(Skill skill)
  {
    try
    {
      _context.Entry(skill).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

}
