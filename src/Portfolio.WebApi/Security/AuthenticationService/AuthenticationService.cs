using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO.TokenDtos;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Models;
using Portfolio.WebApi.Security.Token;

namespace Portfolio.WebApi.Security.AuthenticationService;
public class AuthenticationService : IAuthenticationService
{
  private readonly PortfolioContext _context;
  private readonly IPasswordHasher<User> _passwordHasher;
  private readonly ITokenHandler _tokenHandler;

  public AuthenticationService(PortfolioContext context,
    IPasswordHasher<User> passwordHasher,
    ITokenHandler tokenHandler)
  {
    _tokenHandler = tokenHandler;
    _passwordHasher = passwordHasher;
    _context = context;
  }

  public async Task<AccessToken> CreateToken(UserCredentialsDto userCredentials)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userCredentials.UserName);
    if (user == null)
    {
      throw new RequestException(400, "Invalid credentials");
    }

    PasswordVerificationResult hashResult = _passwordHasher.VerifyHashedPassword(user, user.Password, userCredentials.Password);
    if (hashResult == PasswordVerificationResult.Failed)
    {
      throw new RequestException(400, "Invalid credentials");
    }

    return _tokenHandler.CreateAccessToken(user);
  }

  public async Task<AccessToken> RefreshToken(RefreshTokenRequestDto refreshTokenResource)
  {
    // removes the saved refresh token from the list
    RefreshToken newRefreshToken = _tokenHandler.DrawRefreshToken(refreshTokenResource);

    if (newRefreshToken == null)
    {
      throw new RequestException(400, "Invalid token");
    }

    User user = await GetUser(refreshTokenResource.UserName);

    string errorMessage = RefreshTokenValidator(newRefreshToken, user);

    if (!string.IsNullOrEmpty(errorMessage))
    {
      throw new RequestException(400, errorMessage);
    }
    // creates both an AccessToken and a RefreshToken
    return _tokenHandler.CreateAccessToken(user);
  }

  private static string RefreshTokenValidator(RefreshToken refreshToken, User user)
  {
    string errorMessage = "";
    if (refreshToken == null)
    {
      errorMessage = "Invalid credentials";
    }
    if (refreshToken.IsExpired())
    {
      errorMessage = "Expired refresh token";
    }
    if (user == null)
    {
      errorMessage = "Invalid refresh token";
    }
    return errorMessage;
  }

  private async Task<User> GetUser(string username)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
  }

  public bool RevokeRefreshToken(RefreshTokenRequestDto refreshTokenResource)
  {
    return _tokenHandler.RevokeRefreshToken(refreshTokenResource);
  }
}