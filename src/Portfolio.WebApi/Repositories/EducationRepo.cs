using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class EducationRepo : IService<Education, EducationSearcheable>
{
  private readonly PortfolioContext _context;

  public EducationRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Education>> GetAll()
  {
    try
    {
      return await _context.Educations.ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public IEnumerable<Education> Filter(IEnumerable<Education> educations, EducationSearcheable searchObj)
  {
    if (!string.IsNullOrEmpty(searchObj.Degree))
    {
      educations = educations.Where(e => e.Degree.Contains(searchObj.Degree.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.School))
    {
      educations = educations.Where(e => e.School.Contains(searchObj.School.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.StartDate))
    {
      educations = educations.Where(e => e.StartDate.Contains(searchObj.StartDate.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.EndDate))
    {
      educations = educations.Where(e => e.EndDate.Contains(searchObj.EndDate.Trim()));
    }
    return educations;
  }

  public async Task<Education> GetById(Guid id)
  {
    Education foundEducation = await _context.Educations
      .Include(e => e.User)
      .FirstOrDefaultAsync(e => e.Id == id);
    return foundEducation ?? throw new RequestException(404);
  }

  public async Task Create(Education education)
  {
    try
    {
      _context.Educations.Add(education);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Education education)
  {
    try
    {
      _context.Educations.Remove(education);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }


  public async Task Update(Education education)
  {
    try
    {
      _context.Entry(education).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}