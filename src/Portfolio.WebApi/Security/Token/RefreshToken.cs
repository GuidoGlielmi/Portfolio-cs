
namespace Portfolio.WebApi.Security.Token;

public class RefreshToken : JsonWebToken
{
  // A refresh token is a special non-jwt that is used to obtain additional access tokens
  public RefreshToken() : base() { }
  public RefreshToken(string token, DateTime expiration) : base(token, expiration) { }
  public RefreshToken(string token) : base(token) { }
}