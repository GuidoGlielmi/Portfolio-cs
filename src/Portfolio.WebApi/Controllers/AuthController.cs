using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.WebApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
  public IConfiguration Config { get; }

  public PortfolioContext Context { get; }


  public AuthController(IConfiguration config, PortfolioContext context)
  {
    Config = config;
    Context = context;
  }
  public class UserCredentials
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }

  public class AuthenticatedResponseObject
  {
    public string Token { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }


  [HttpPost]
  public async Task<ActionResult<ResponseDto<AuthenticatedResponseObject>>> Authenticate(UserCredentials credentials)
  {
    // OAuth2
    // OpenID Connnect indentity layer on top of OAuth2 protocol
    UserPutDto user = await ValidateUserCredentials(credentials);

    if (user == null)
    {
      return StatusCode(401, new RequestException(401).Error);
    }

    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config["Authentication:SecretForKey"]));
    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claimsForToken = new List<Claim>
    {
      // these are standard name values
      // when using conventional (standard) claim types like "given_name", the enum ClaimTypes MUST be used
      new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
      new Claim(ClaimTypes.GivenName, user.FirstName),
      new Claim(ClaimTypes.Surname, user.LastName),
      new Claim(ClaimTypes.Role, user.Role.RoleName.ToString())
    };

    var jwtSecurityToken = new JwtSecurityToken(
      Config["Authentication:Issuer"],
      Config["Authentication:Audience"],
      claimsForToken,
      DateTime.UtcNow,
      DateTime.UtcNow.AddHours(1),
      signingCredentials
    );

    string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

    var responseObject = new AuthenticatedResponseObject
    {
      Token = tokenToReturn,
      FirstName = user.FirstName,
      LastName = user.LastName
    };

    // tokens are not encrypted but encoded, so token-based security COMPLETELY relies on https for encryption

    return new ResponseDto<AuthenticatedResponseObject>(responseObject, "Logged in successfully");
  }

  private async Task<UserPutDto> ValidateUserCredentials(UserCredentials user)
  {
    User foundUser = await Context.Users.FirstOrDefaultAsync(u => u.Username == user.UserName);
    if (foundUser.Password == user.Password)
    {
      return new UserPutDto
      {
        Id = foundUser.Id,
        Username = foundUser.Username,
        Password = foundUser.Password,
        FirstName = foundUser.FirstName,
        LastName = foundUser.LastName,
        AboutMe = foundUser.AboutMe,
        ProfileImg = foundUser.ProfileImg,
        GithubUrl = foundUser.GithubUrl,
        LinkedInUrl = foundUser.LinkedInUrl,
        Role = foundUser.Role,
      };
    }
    return null;
  }
}
