using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.IRepositories;

public interface IProjectService<T, TSearcheable> : IPortfolioService<T, TSearcheable>
  where T : class
  where TSearcheable : class
{
  Task<List<Technology>> GetTechs(List<Guid> ids);
}
