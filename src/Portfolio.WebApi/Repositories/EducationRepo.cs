using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class EducationRepo : IPortfolioService<Education, EducationSearcheable>
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

  public async Task<Education> GetById(Guid id)
  {
    Education foundEducation = (await GetAll()).FirstOrDefault(e => e.Id == id);
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


//public override IEnumerable<Education> Filter(IEnumerable<Education> educations, EducationSearcheable searchObj)
//{
//  var searchObjKeyValuePair = ToDictionary<string>(searchObj);

//  foreach (var (key, value) in searchObjKeyValuePair)
//  {
//    educations = educations.DynamicWhere(key, value);
//  }

//  return educations;
//}