using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class SkillRepo : IService<Skill, SkillSearcheable>
{
  public PortfolioContext Context { get; }

  public SkillRepo(PortfolioContext context)
  {
    Context = context;
  }

  public async Task<IEnumerable<Skill>> GetAll()
  {
    try
    {
      return await Context.Skills.ToListAsync();
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
    Skill foundSkill = await Context.Skills.FindAsync(id);
    return foundSkill ?? throw new RequestException(404);
  }

  public async Task Create(Skill skill)
  {
    try
    {
      Context.Skills.Add(skill);
      await Context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Skill skill)
  {
    try
    {
      Context.Skills.Remove(skill);
      await Context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(Skill skill)
  {
    try
    {
      Context.Entry(skill).State = EntityState.Modified;
      await Context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

}
