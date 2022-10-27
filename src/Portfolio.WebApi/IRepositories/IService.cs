namespace Portfolio.WebApi.IRepositories;

public interface IService<T, TSearcheable>
  where T : class
  where TSearcheable : class
{
  Task<IEnumerable<T>> GetAll();

  Task<T> GetById(Guid id);

  Task Create(T entity);

  Task Update(T entity);

  Task Delete(T entity);

  IEnumerable<T> Filter(IEnumerable<T> entities, TSearcheable searchObj);
}
