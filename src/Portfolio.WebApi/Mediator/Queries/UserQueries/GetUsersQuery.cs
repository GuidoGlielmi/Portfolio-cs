using Portfolio.WebApi.DTO.UserDtos;

namespace Portfolio.WebApi.Mediator.Queries.UserQueries;

public record GetUsersQuery(Dictionary<string, string> SearchObj) : IQuery<IEnumerable<UserPutDto>> { }