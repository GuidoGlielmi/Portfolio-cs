using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Options;

namespace Portfolio.WebApi.AuthorizationRequirements;
// the name of the class must correspond to a handler that is used for this policy
public class UserResourceRequirement : IAuthorizationRequirement
{
  // a requirement is used mainly to pass parameters when annotating with the authorize class
  public string Name { get; set; }
  public UserResourceRequirement()
  {

  }
  // a constructor can be used to pass values used in authorization
  public const string Policy = "UserResource";
}

public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
  private readonly IAuthorizationMiddlewareResultHandler _defaultHandler;

  public SampleAuthorizationMiddlewareResultHandler(IAuthorizationMiddlewareResultHandler defaultHandler)
  {
    _defaultHandler = defaultHandler;
  }
  public async Task HandleAsync(
      RequestDelegate next,
      HttpContext context,
      AuthorizationPolicy policy,
      PolicyAuthorizationResult authorizeResult)
  {
    // If the authorization was forbidden and the resource had a specific requirement,
    // provide a custom 404 response.
    if (authorizeResult.Forbidden
        && authorizeResult.AuthorizationFailure!.FailedRequirements
            .OfType<UserResourceRequirement>()
            .Any())
    {
      // Return a 404 to make it appear as if the resource doesn't exist.
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      return;
    }

    // Fall back to the default implementation.
    await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
  }
}







public static class Operations
{
  public static OperationAuthorizationRequirement Create =
      new()
      { Name = nameof(Create) };
  public static OperationAuthorizationRequirement Read =
      new()
      { Name = nameof(Read) };
  public static OperationAuthorizationRequirement Update =
      new()
      { Name = nameof(Update) };
  public static OperationAuthorizationRequirement Delete =
      new()
      { Name = nameof(Delete) };
}

public class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
  const string POLICY_PREFIX = "MinimumAge";
  public MinimumAgeAuthorizeAttribute(int age) => Age = age;
  public int Age
  {
    get
    {
      return int.TryParse(Policy[POLICY_PREFIX.Length..], out var age) ? age : default;
    }
    set => Policy = $"{POLICY_PREFIX}{value}";
    // this is where the policy name, from the Authorize attribute, is set.
    // this will be checked against the registered policies to match the corresponding built policy.
    // alternatively, the policy will be picked up by the policy provider to
    // build the related policy
  }
}

public class MinimumAgeRequirement : IAuthorizationRequirement
{
  public MinimumAgeRequirement(int minimumAge) =>
      MinimumAge = minimumAge;

  public int MinimumAge { get; }
}

internal class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
{
  // Used to retrieve authorization policies.
  //ASP.NET Core only uses one instance of IAuthorizationPolicyProvider.
  //If a custom provider isn't able to provide authorization policies for all policy names that will be used,
  //it should defer to a backup provider.

  /*
  To use custom policies from an IAuthorizationPolicyProvider, you must:
    - Register the appropriate AuthorizationHandler types with dependency injection, as with all policy-based authorization scenarios.
    - Register the custom IAuthorizationPolicyProvider type to replace the default policy provider.
  */

  // it builds all the necessary policies from each authorized attribute used
  // [Authorize("MinimumAge21")]
  // [Authorize("MinimumAge18")]
  // [Authorize("MinimumAge13")]
  // ...
  // this is done to avoid having to add the authorization policies by hand:
  // services.AddAuthorization(opt =>
  // {
  //   opt.AddPolicy("policy1", policy => policy.Requirements.Add(new UserResourceRequirement(1)));
  //   opt.AddPolicy("policy2", policy => policy.Requirements.Add(new UserResourceRequirement(2)));
  //   ...
  // });
  // instead, when [Authorize("MinimumAge<age>")] or [MinimumAge(age)] is used, the policy is added programatically
  const string POLICY_PREFIX = "MinimumAge";

  public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
  {
    //a custom IAuthorizationPolicyProvider needs to implement GetDefaultPolicyAsync
    //to provide an authorization policy for [Authorize] attributes without a policy name specified.
    return Task.FromResult(new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
  }

  public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
  {
    throw new NotImplementedException();
  }
  public DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
  public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
  {
    // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
    // doesn't handle all policies it should fall back to an alternate provider.
    BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
  }

  public Task<AuthorizationPolicy> GetPolicyAsync(string policyName) // MinimumAge21
  {
    if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
        int.TryParse(policyName[POLICY_PREFIX.Length..], out var age))
    {
      var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
      policy.AddRequirements(new MinimumAgeRequirement(age));
      return Task.FromResult(policy.Build());
    }

    return BackupPolicyProvider.GetPolicyAsync(policyName); // return backup policy
  }
}