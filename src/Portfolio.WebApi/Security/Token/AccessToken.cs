
namespace Portfolio.WebApi.Security.Token;

public class AccessToken : JsonWebToken
{
  public string RefreshToken { get; private set; }
  public string FirstName { get; }
  public string LastName { get; }

  public AccessToken() : base() { }

  public AccessToken(string token, DateTime expiration, string refreshToken, string firstName, string lastName) : base(token, expiration)
  {
    RefreshToken = refreshToken ?? throw new ArgumentException("Specify a valid refresh token.");
    FirstName = firstName;
    LastName = lastName;
  }
}

