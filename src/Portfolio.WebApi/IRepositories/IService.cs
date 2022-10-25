namespace Portfolio.WebApi.IRepositories;

public interface IService<T, TSearcheable>
  where T : class
  where TSearcheable : class
{
  Task<IEnumerable<T>> GetAll();

  IEnumerable<T> Filter(IEnumerable<T> entities, TSearcheable searchObj);

  Task<T> GetById(Guid id);

  Task Create(T entity);

  Task Update(T entity);

  Task Delete(T entity);

  //Task Patch(T entity);
}
