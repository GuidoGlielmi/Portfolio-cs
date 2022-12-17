using Portfolio.WebApi.DTO.SkillDtos;

namespace Portfolio.WebApi.Mediator.Queries.SkillQueries;

public record GetSkillsQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<SkillPutDto>> { }
