using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO.TokenDtos;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Security.AuthenticationService;
using Portfolio.WebApi.Security.Token;

namespace Portfolio.WebApi.Controllers;
[Route("api/login")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IMapper _mapper;

  private readonly IAuthenticationService _authenticationService;

  public AuthController(IMapper mapper, IAuthenticationService authenticationService)
  {
    _mapper = mapper;
    _authenticationService = authenticationService;
  }

  [HttpPost]
  public async Task<ActionResult<AccessToken>> Login(UserCredentialsDto userCredentials)
  {
    AccessToken token = await _authenticationService.CreateToken(userCredentials);
    return Ok(token);
  }

  [Route("/refresh")]
  [HttpPost]
  public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenResource)
  {
    // the refresh token is the user's password hashed. The only verification to generate another token
    // is to find it in the refresh token list saved in the server. The new refresh token replaces
    // the old one, as the hash generator adds salt to not create alwatys the same result 
    AccessToken token = await _authenticationService.RefreshToken(refreshTokenResource);

    // AccessToken -> Token, Expiration, RefreshToken, FirstName, LastName
    // RefreshTokenResponseDto -> Token, Expiration, RefreshToken
    return Ok(_mapper.Map<RefreshTokenResponseDto>(token));
  }

  [Route("/revoke")]
  [HttpPost]
  public IActionResult RevokeToken(RefreshTokenRequestDto revokeTokenResource)
  {
    _authenticationService.RevokeRefreshToken(revokeTokenResource);
    return NoContent();
  }
}









// if an attacker can get access to a token (which is easy if they can create an account), then the may be able
// to manipulate the tokens content and present it as valid if they can determine the secret key.

// secrets need to be at least 43 alphanumeric characters long.

// assymmetric keys that use a public/private key par are often used for cross-server applications.
// With this approach the token-issuing server signs tokens using a private key,
// and other servers can validate its signature by using the public key. OpenID Connect uses this approach.

// once issued, JWT's are immutable and can be rendered invalid
//[HttpPost]
//public async Task<ActionResult<AuthenticatedResponseObject>> Authenticate(UserCredentials credentials)
//{
//  // OAuth2
//  // OpenID Connnect indentity layer on top of OAuth2 protocol
//  UserPutDto user = await ValidateUserCredentials(credentials);

//  if (user == null)
//  {
//    return StatusCode(401, ((HttpStatusCode)401).ToString());
//  }

//  string tokenToReturn = GetToken(user);

//  var responseObject = new AuthenticatedResponseObject
//  {
//    Token = tokenToReturn,
//    FirstName = user.FirstName,
//    LastName = user.LastName
//  };

//  // tokens are not encrypted but encoded, so token-based security COMPLETELY relies on https for encryption

//  return responseObject;
//}

//  private string GetToken(UserPutDto user)
//  {
//    List<Claim> claimsForToken = GetClaims(user);

//    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));
//    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//    var jwtSecurityToken = new JwtSecurityToken(
//      issuer: _config["Authentication:Issuer"],
//      audience: _config["Authentication:Audience"],
//      claims: claimsForToken,
//      notBefore: DateTime.UtcNow,
//      expires: DateTime.UtcNow.AddHours(1),
//      signingCredentials
//    );

//    string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//    return tokenToReturn;
//  }

//  private static List<Claim> GetClaims(UserPutDto user)
//  {
//    var claimsForToken = new List<Claim>
//    {
//      // these are standard name values
//      // when using conventional (standard) claim types like "given_name", the enum ClaimTypes MUST be used
//      new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
//      new Claim(ClaimTypes.GivenName, user.FirstName),
//      new Claim(ClaimTypes.Surname, user.LastName),
//      new Claim(ClaimTypes.Role, user.Role.RoleName.ToString())
//    };
//    return claimsForToken;
//  }

//  private async Task<UserPutDto> ValidateUserCredentials(UserCredentialsDto user)
//  {
//    User foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.UserName);
//    if (foundUser == null || foundUser.Password != user.Password)
//    {
//      return null;
//    }
//    return _mapper.Map<UserPutDto>(user);
//  }
