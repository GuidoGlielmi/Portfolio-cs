namespace Portfolio.WebApi.Security.Token;

public class TokenOptions
{
  // must match the "TokenOptions" in appsettings
  // this is a template for creating new tokens, so it can't have absolute values
  public string Audience { get; set; }
  public string Issuer { get; set; }
  public int AccessTokenExpiration { get; set; } = 1;
  public int RefreshTokenExpiration { get; set; } = 1;
  public string Secret { get; set; }
}