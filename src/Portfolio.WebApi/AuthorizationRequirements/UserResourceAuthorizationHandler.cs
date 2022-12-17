using Microsoft.AspNetCore.Authorization;
using Portfolio.WebApi.DTO.EducationDtos;
using System.Security.Claims;

namespace Portfolio.WebApi.AuthorizationRequirements;
public class UserResourceAuthorizationHandler : AuthorizationHandler<UserResourceRequirement, EducationPutDto>
{
  private readonly ILogger _logger;

  /*
An authorization handler is responsible for the evaluation of a requirement's properties.
The authorization handler evaluates the requirements against a provided AuthorizationHandlerContext to determine if access is allowed. 
Alternatively, a handler may implement IAuthorizationHandler directly to handle more than one type of requirement.
*/
  public UserResourceAuthorizationHandler(ILoggerFactory loggerFactory)
      => _logger = loggerFactory.CreateLogger(GetType().FullName);
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserResourceRequirement requirementToAccessResource, EducationPutDto protectedResource)
  {
    var userFullName = context.User.Claims
      .Where(c => c.Type is ClaimTypes.Name or ClaimTypes.Surname)
      .Select(c => c.Value)
      .Aggregate((p, c) => p + " " + c);

    _logger.LogInformation("{userFullName} tried to log in at {currentTime}", userFullName, DateTime.UtcNow.ToShortTimeString());

    var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid);
    if (idClaim == null)
    {
      return Task.CompletedTask;
    }
    var resourceBelongToRequestingUser = idClaim.Value == protectedResource.UserId.ToString();
    var userIsAdmin = context.User.IsInRole(UserResourceRequirement.Policy);
    if (resourceBelongToRequestingUser && userIsAdmin!)
    {
      context.Succeed(requirementToAccessResource);
    }
    return Task.CompletedTask;
  }
}
