using Portfolio.WebApi.DTO.ExperienceDtos;

namespace Portfolio.WebApi.Mediator.Queries.ExperienceQueries;

public record GetExperiencesQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<ExperiencePutDto>> { }