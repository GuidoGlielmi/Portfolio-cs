using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Repositories;

public class UserRepo : IService<User, UserSearcheable>
{
  private readonly PortfolioContext _context;

  public UserRepo(PortfolioContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<User>> GetAll()
  {
    try
    {
      return await _context.Users.ToListAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public IEnumerable<User> Filter(IEnumerable<User> users, UserSearcheable searchObj)
  {
    if (!string.IsNullOrEmpty(searchObj.Username))
    {
      users = users.Where(u => u.Username.ToLower() == searchObj.Username.ToLower());
    }
    if (!string.IsNullOrEmpty(searchObj.FirstName))
    {
      users = users.Where(u => u.FirstName.ToLower() == searchObj.FirstName.ToLower());
    }
    if (!string.IsNullOrEmpty(searchObj.LastName))
    {
      users = users.Where(u => u.LastName.ToLower() == searchObj.LastName.ToLower());
    }
    if (!string.IsNullOrEmpty(searchObj.GithubUrl))
    {
      users = users.Where(u => u.GithubUrl.Contains(searchObj.GithubUrl));
    }
    if (!string.IsNullOrEmpty(searchObj.LinkedInUrl))
    {
      users = users.Where(u => u.LinkedInUrl.Contains(searchObj.LinkedInUrl));
    }

    return users;
  }

  public async Task<User> GetById(Guid id)
  {
    User foundUser = await _context.Users.FindAsync(id);
    return foundUser ?? throw new RequestException(404);
  }

  public async Task Create(User user)
  {
    try
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Delete(User user)
  {
    try
    {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
    } catch (Exception)
    {
      throw new RequestException(500);
    }
  }

  public async Task Update(User user)
  {
    try
    {
      _context.Entry(user).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    } catch (DbUpdateException)
    {
      throw new RequestException(500);
    }
  }


}
