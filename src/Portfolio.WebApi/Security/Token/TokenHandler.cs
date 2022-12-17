using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Portfolio.WebApi.DTO.TokenDtos;
using Portfolio.WebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Portfolio.WebApi.Security.Token;

public class TokenHandler : ITokenHandler
{
  private readonly ISet<RefreshTokenWithUsername> _refreshTokens = new HashSet<RefreshTokenWithUsername>();

  private readonly TokenOptions _tokenOptions; // service included
  private readonly SigningConfigurations _signingConfigurations; // service included
  private readonly IPasswordHasher<User> _passwordHasher; // service included

  public TokenHandler(IOptions<TokenOptions> tokenOptionsSnapshot,
    // via .Configure<TokenOptions> a singleton is created from an object in appsettings.Development.json 
    // and can be used to read configuration data within any service lifetime.
    // Being singleton, it cannot read changes to the configuration data after the app has started.
    SigningConfigurations signingConfigurations,
    IPasswordHasher<User> passwordHasher)
  {
    _passwordHasher = passwordHasher;
    _tokenOptions = tokenOptionsSnapshot.Value;
    _signingConfigurations = signingConfigurations;
  }


  public AccessToken CreateAccessToken(User user)
  {
    var refreshToken = BuildRefreshToken(user); // RefreshToken is virtually the same as AccessToken
    _refreshTokens.Add(new RefreshTokenWithUsername
    {
      Username = user.Username,
      RefreshToken = refreshToken
    });

    return BuildAccessToken(user, refreshToken);
  }


  /// <summary>
  /// Creates a Resfresh token with the user's password
  /// </summary>
  private RefreshToken BuildRefreshToken(User user)
  {
    return new RefreshToken
    (
        token: _passwordHasher.HashPassword(user, user.Password),
        // This function do hashing with salt.
        // This is a techique to make sure it yield different hash for the same password and make table lookup much harder.
        expiration: DateTime.UtcNow.AddHours(_tokenOptions.RefreshTokenExpiration)
    );
  }

  private AccessToken BuildAccessToken(User user, RefreshToken refreshToken)
  {
    var securityToken = new JwtSecurityToken
    (
        issuer: _tokenOptions.Issuer,
        audience: _tokenOptions.Audience,
        claims: GetClaims(user),
        expires: DateTime.UtcNow.AddHours(_tokenOptions.AccessTokenExpiration), // AccessTokenExpiration is in Ticks
        notBefore: DateTime.UtcNow,
        signingCredentials: _signingConfigurations.SigningCredentials
    );

    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

    return new AccessToken(
      token,
      expiration: DateTime.UtcNow.AddHours(_tokenOptions.AccessTokenExpiration),
      refreshToken.Token,
      firstName: user.FirstName,
      lastName: user.LastName);
  }

  private static IEnumerable<Claim> GetClaims(User user)
  {
    return new List<Claim>
    {
      // these are standard name values
      // when using conventional (standard) claim types like "given_name", the enum ClaimTypes MUST be used
      new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
      new Claim(ClaimTypes.GivenName, user.FirstName),
      new Claim(ClaimTypes.Surname, user.LastName),
      new Claim(ClaimTypes.Role, user.Role.RoleName.ToString())
    };
  }


  public RefreshToken DrawRefreshToken(RefreshTokenRequestDto refreshTokenRequest)
  {
    if (!ValidateRefreshToken(refreshTokenRequest))
    {
      return null;
    }

    var savedRefreshToken = GetSavedRefreshToken(refreshTokenRequest); // Checks if the token is saved

    return savedRefreshToken?.RefreshToken;
  }

  private static bool ValidateRefreshToken(RefreshTokenRequestDto refreshTokenRequest)
  {
    if (string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken))
      return false;
    if (string.IsNullOrWhiteSpace(refreshTokenRequest.UserName))
      return false;

    return true;
  }

  public bool RevokeRefreshToken(RefreshTokenRequestDto refreshTokenRequest)
  {
    return DrawRefreshToken(refreshTokenRequest) != null;
  }

  /// <summary>
  /// Checks if the token is saved
  /// </summary>
  private RefreshTokenWithUsername GetSavedRefreshToken(RefreshTokenRequestDto refreshTokenRequest)
  {

    RefreshTokenWithUsername savedRefreshToken = _refreshTokens
      .SingleOrDefault(rt =>
        rt.RefreshToken.Token == refreshTokenRequest.RefreshToken && rt.Username == refreshTokenRequest.UserName);
    if (savedRefreshToken == null)
      return null;

    _refreshTokens.Remove(savedRefreshToken);

    return savedRefreshToken;
  }

  private class RefreshTokenWithUsername
  {
    public string Username { get; set; }
    public RefreshToken RefreshToken { get; set; }
  }
}