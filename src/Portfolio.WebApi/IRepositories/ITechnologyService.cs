using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.IRepositories;

public interface ITechnologyService<T, TSearcheable> : IPortfolioService<T, TSearcheable>
  where T : class
  where TSearcheable : class
{
  Task<List<Project>> GetProjects(List<Guid> ids);
}
