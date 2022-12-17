using Newtonsoft.Json;

namespace Portfolio.WebApi.IRepositories;

public interface IPortfolioService<T, TSearcheable>
  where T : class
  where TSearcheable : class
{
  Task<IEnumerable<T>> GetAll();

  Task<T> GetById(Guid id);

  Task Create(T entity);

  Task Update(T entity);

  Task Delete(T entity);

}
//IEnumerable<T> Filter(IEnumerable<T> entities, TSearcheable searchObj);

//IEnumerable<KeyValuePair<string, TValue>> ToDictionary<TValue>(object obj)
//{
//  var asd = new Dictionary<string, string>();
//  var json = JsonConvert.SerializeObject(obj);
//  var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
//  return dictionary.Where((kvp) => kvp.Value != null);
//}
