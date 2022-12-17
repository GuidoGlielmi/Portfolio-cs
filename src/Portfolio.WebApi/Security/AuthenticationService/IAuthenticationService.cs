using Portfolio.WebApi.DTO.TokenDtos;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Security.Token;
namespace Portfolio.WebApi.Security.AuthenticationService;

public interface IAuthenticationService
{
  /// <summary>
  /// Creates a <see cref="AccessToken"/> and saves its <see cref="Token.RefreshToken"/>
  /// </summary>
  Task<AccessToken> CreateToken(UserCredentialsDto userCredentials);

  /// <summary>
  /// If found, replaces <paramref name="refreshTokenResource"/> with a new one and returns a new token
  /// </summary>
  Task<AccessToken> RefreshToken(RefreshTokenRequestDto refreshTokenResource);

  /// <summary>
  /// Discards the saved <see cref="Token.RefreshToken"/>
  /// </summary>
  bool RevokeRefreshToken(RefreshTokenRequestDto refreshToken);
}