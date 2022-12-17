using Portfolio.WebApi.DTO.TokenDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Security.Token;

public interface ITokenHandler
{
  /// <summary>
  /// Builds an <see cref="AccessToken"/> and saves its corresponding <see cref="RefreshToken"/>
  /// </summary>
  /// <returns><see cref="AccessToken"/></returns>
  AccessToken CreateAccessToken(User user);

  /// <summary>
  /// If found, discards the saved <see cref="RefreshToken"/> instance and returns it
  /// </summary>
  /// <returns><see cref="RefreshToken"/></returns>
  RefreshToken DrawRefreshToken(RefreshTokenRequestDto refreshTokenRequest);

  /// <summary>
  /// If found, discards the saved <see cref="RefreshToken"/> instance
  /// </summary>
  bool RevokeRefreshToken(RefreshTokenRequestDto refreshTokenRequest);
}