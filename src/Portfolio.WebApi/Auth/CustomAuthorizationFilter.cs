//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization.Policy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Portfolio.WebApi.DTO;

//namespace Portfolio.WebApi.Auth;

//public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
//{
//  public AuthorizationPolicy Policy { get; } = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

//  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//  {
//    if (context == null)
//    {
//      throw new ArgumentNullException(nameof(context));
//    }

//    // Allow Anonymous skips all authorization
//    if (context.Filters.Any(item => item is IAllowAnonymousFilter))
//    {
//      return;
//    }

//    var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
//    var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
//    var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

//    if (authorizeResult.Challenged)
//    {
//      // Return custom 401 result
//      var responseObj = new ResponseDto<string>(400, "Must be logged in to perform that action");

//      context.Result = new JsonResult(responseObj)
//      {
//        StatusCode = 401
//      };
//    }
//  }
//}