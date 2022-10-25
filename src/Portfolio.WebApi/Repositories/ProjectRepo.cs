using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.ProjectDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class ProjectRepo : IProjectService<Project, ProjectSearcheable>
{
  private readonly PortfolioContext _context;

  public ProjectRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Project>> GetAll()
  {
    try
    {
      return await _context.Projects
      .Include(p => p.Techs)
      .Include(p => p.Urls)
      .ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public IEnumerable<Project> Filter(IEnumerable<Project> projects, ProjectSearcheable searchObj)
  {
    if (!string.IsNullOrEmpty(searchObj.Title))
    {
      projects = projects.Where(p => p.Title.Contains(searchObj.Title.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.DeployUrl))
    {
      projects = projects.Where(p => p.DeployUrl.Contains(searchObj.DeployUrl.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.Description))
    {
      projects = projects.Where(p => p.Description.Contains(searchObj.Description.Trim()));
    }
    if (!string.IsNullOrEmpty(searchObj.Url))
    {
      projects = projects.Where(p => p.Urls.Any(u => u.Url.Contains(searchObj.Description.Trim())));
    }

    return projects;
  }

  public async Task<Project> GetById(Guid id)
  {
    Project foundProject = await _context.Projects
      .Include(p => p.User)
      .Include(p => p.Techs)
      .Include(p => p.Urls)
      .FirstOrDefaultAsync(p => p.Id == id);
    return foundProject ?? throw new RequestException(404);
  }

  public async Task Create(Project project)
  {
    try
    {
      _context.Projects.Add(project);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(Project project)
  {
    try
    {
      _context.Projects.Remove(project);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(Project project)
  {
    try
    {
      _context.Entry(project).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }

  public async Task<List<Technology>> GetTechs(List<Guid> ids)
  {
    try
    {
      var techs = new List<Technology>();
      foreach (Guid id in ids)
      {
        techs.Add(await _context.Technologies.FindAsync(id));
      }
      return techs;
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }
}
