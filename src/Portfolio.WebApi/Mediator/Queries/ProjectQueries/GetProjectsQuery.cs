using Portfolio.WebApi.DTO.ProjectDtos;

namespace Portfolio.WebApi.Mediator.Queries.ProjectQueries;

public record GetProjectsQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<ProjectPutDto>> { }