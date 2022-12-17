using Portfolio.WebApi.DTO.TechnologyDtos;

namespace Portfolio.WebApi.Mediator.Queries.TechnologyQueries;

public record GetTechnologiesQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<TechnologyPutDto>> { }

