using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.Experience;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class ExperienceRepo : IPortfolioService<Experience, ExperienceSearcheable>
{
  private readonly PortfolioContext _context;

  public ExperienceRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Experience>> GetAll()
  {
    try
    {
      return await _context.Experiences.ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task<Experience> GetById(Guid id)
  {
    Experience foundExperience = await _context.Experiences.FindAsync(id);
    return foundExperience ?? throw new RequestException(404);
  }

  public async Task Create(Experience experience)
  {
    try
    {
      _context.Experiences.Add(experience);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Experience experience)
  {
    try
    {
      _context.Experiences.Remove(experience);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(Experience experience)
  {
    try
    {
      _context.Entry(experience).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

}


//public IEnumerable<Experience> Filter(IEnumerable<Experience> experiences, ExperienceSearcheable searchObj)
//{
//  if (!string.IsNullOrEmpty(searchObj.Description))
//  {
//    experiences = experiences.Where(e => e.Description.Contains(searchObj.Description.Trim()));
//  }
//  if (!string.IsNullOrEmpty(searchObj.Title))
//  {
//    experiences = experiences.Where(e => e.Title.Contains(searchObj.Title.Trim()));
//  }
//  if (!string.IsNullOrEmpty(searchObj.StartDate))
//  {
//    experiences = experiences.Where(e => e.StartDate.Contains(searchObj.StartDate.Trim()));
//  }
//  if (!string.IsNullOrEmpty(searchObj.EndDate))
//  {
//    experiences = experiences.Where(e => e.EndDate.Contains(searchObj.EndDate.Trim()));
//  }

//  return experiences;
//}
