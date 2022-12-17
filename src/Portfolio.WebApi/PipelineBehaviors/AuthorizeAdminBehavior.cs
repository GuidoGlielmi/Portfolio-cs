using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Mediator.Commands;
using Portfolio.WebApi.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using TokenOptions = Portfolio.WebApi.Security.Token.TokenOptions;

namespace Portfolio.WebApi.PipelineBehaviors;

public class AuthorizeAdminBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
  private readonly HttpContext _httpContext;
  private readonly SigningConfigurations _signingConfigurations;
  private readonly TokenOptions _tokenOptions;

  public AuthorizeAdminBehavior(IHttpContextAccessor httpContext,
    SigningConfigurations signingConfigurations,
    IOptions<TokenOptions> tokenOptionsSnapshot)
  {
    _httpContext = httpContext.HttpContext;
    _signingConfigurations = signingConfigurations;
    _tokenOptions = tokenOptionsSnapshot.Value;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var tokenValidationParameters = new TokenValidationParameters()
    {
      ValidateAudience = true, // true by default
      ValidateLifetime = true, // true by default
      ValidateIssuer = true, // true by default
      ValidateIssuerSigningKey = true, // true by default
      ValidIssuer = _tokenOptions.Issuer,
      ValidAudience = _tokenOptions.Audience,
      IssuerSigningKey = _signingConfigurations.SecurityKey,
      ClockSkew = TimeSpan.Zero // expire exactly at token expiration time (instead of 5 minutes later)
    };
    StringValues token = _httpContext.Request.Headers.Authorization;
    if (!token.First().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
    {
      throw new RequestException(401, "Provide a token");
    }
    try
    {
      var claims = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
      if (!claims.IsInRole("ADMIN"))
      {
        //throw new RequestException(403, "Unauthorized member");
        throw new RequestException(404, "Not found"); // used to hide the existence of a resource
      }
    } catch (RequestException)
    {
      throw;
    } catch (Exception)
    {
      throw new RequestException(401, "Invalid token");
    }
    return await next();
  }
}