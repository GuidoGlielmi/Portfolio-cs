using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.TechnologyDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class TechnologyRepo : ITechnologyService<Technology, TechnologySearcheable>
{
  private readonly PortfolioContext _context;

  public TechnologyRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Technology>> GetAll()
  {
    try
    {
      return await _context.Technologies.ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public IEnumerable<Technology> Filter(IEnumerable<Technology> techs, TechnologySearcheable searchObj)
  {
    if (!string.IsNullOrEmpty(searchObj.Name))
    {
      techs = techs.Where(t => t.Name.ToString().ToLower() == searchObj.Name.ToLower());
    }

    return techs;
  }
  public async Task<Technology> GetById(Guid id)
  {
    Technology foundTechnology = await _context.Technologies.FindAsync(id);
    return foundTechnology ?? throw new RequestException(404);
  }

  public async Task Create(Technology technology)
  {
    try
    {
      _context.Technologies.Add(technology);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Technology technology)
  {
    try
    {
      _context.Technologies.Remove(technology);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(Technology technology)
  {
    try
    {
      _context.Entry(technology).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

  public async Task<List<Project>> GetProjects(List<Guid> ids)
  {
    try
    {
      var techs = new List<Project>();
      foreach (Guid id in ids)
      {
        techs.Add(await _context.Projects.FindAsync(id));
      }
      return techs;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

}
