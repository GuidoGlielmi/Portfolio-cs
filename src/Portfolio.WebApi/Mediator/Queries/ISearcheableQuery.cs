using MediatR;

namespace Portfolio.WebApi.Mediator.Queries;

public abstract class SearcheableList<T, TDto> : IRequest<T>
{
	public Dictionary<string, string> SearchObj { get; }
	public IEnumerable<TDto> EntitiyList { get; set; }

	public SearcheableList(Dictionary<string, string> searchObj)
	{
		SearchObj = searchObj;
	}
}

public interface ISearcheableObject { }