using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portfolio.WebApi.DTO;

namespace Portfolio.WebApi.Errors;

public class UnauthorizedResponseActionFilter : IAuthorizationFilter//IAuthorizationMiddlewareResultHandler
{

  public AuthorizationPolicy Policy { get; }

  public UnauthorizedResponseActionFilter(AuthorizationPolicy policy)
  {
    Policy = policy ?? throw new ArgumentNullException(nameof(policy));
  }

  public async void OnAuthorization(AuthorizationFilterContext context)
  {
    //if (context == null)
    //{
    //  throw new ArgumentNullException(nameof(context));
    //}

    //// Allow Anonymous skips all authorization
    //if (context.Filters.Any(item => item is IAllowAnonymousFilter))
    //{
    //  return;
    //}
    var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
    var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
    var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

    if (authorizeResult.Challenged)
    {
      // Return custom 401 result
      var responseObj = new ResponseDto<string>(403, "Unauthorized access");
      context.Result = new JsonResult(responseObj)
      {
        StatusCode = 400
      };
    } else if (authorizeResult.Forbidden)
    {
      // Return default 403 result
      context.Result = new ForbidResult(Policy.AuthenticationSchemes.ToArray());
    }
  }
}
