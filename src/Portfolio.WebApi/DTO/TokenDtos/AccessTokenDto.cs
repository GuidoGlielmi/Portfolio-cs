using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Security.Token;

namespace Portfolio.WebApi.DTO.TokenDtos;

public class AccessTokenDto : IMapFrom<AccessToken>
{
  public string Token { get; set; }
  public string RefreshToken { get; set; }
  public DateTime Expiration { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}