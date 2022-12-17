using Portfolio.WebApi.DTO.EducationDtos;

namespace Portfolio.WebApi.Mediator.Queries.EducationQueries;

public record GetEducationsQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<EducationPutDto>> { }
