
namespace Portfolio.WebApi.Security.Token;

public abstract class JsonWebToken
{
  public string Token { get; protected set; }
  public DateTime Expiration { get; protected set; } = DateTime.UtcNow.AddHours(1);

  public JsonWebToken() { }

  public JsonWebToken(string token, DateTime expiration)
  {
    if (string.IsNullOrWhiteSpace(token))
      throw new ArgumentException("Invalid token.");

    if (expiration <= DateTime.UtcNow)
      throw new ArgumentException("Invalid expiration.");

    Token = token;
    Expiration = expiration;
  }

  public JsonWebToken(string token)
  {
    if (string.IsNullOrWhiteSpace(token))
      throw new ArgumentException("Invalid token.");

    Token = token;
  }

  public bool IsExpired() => DateTime.UtcNow > Expiration;
}