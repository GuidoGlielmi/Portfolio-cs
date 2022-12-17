using Portfolio.WebApi.AuthorizationRequirements;
using Portfolio.WebApi.Security.Token;

namespace Portfolio.WebApi.Extensions;

public static class AuthExtensions
{
  /*
  The authentication handlers registered and their configuration options are called "schemes". 
  If multiple schemes are used, authorization policies (or authorization attributes)
  can specify the authentication scheme (or schemes) they depend on to authenticate the user.
  Calling UseAuthentication registers the middleware that uses the previously registered authentication schemes.
  Call UseAuthentication before any middleware that depends on users being authenticated.
  Authentication is responsible for providing the ClaimsPrincipal for authorization to make permission decisions against.
  When there is only a single authentication scheme registered, it becomes the default scheme.
  If multiple schemes are registered, a default scheme must be specified in the authorize attribute

  An authentication scheme is a name that corresponds to:
  - An authentication handler.
  - Options for configuring that specific instance of the handler.
  Schemes are useful as a mechanism for referring to the behaviors of the associated handler (authentication, challenge, and forbid).

  An authentication handler:

  - Is a type that implements the behavior of a scheme.
  - Is derived from IAuthenticationHandler or AuthenticationHandler<TOptions>.
  - Has the primary responsibility to authenticate users.

  Based on the authentication scheme's configuration and the incoming request context, authentication handlers:

  - Construct AuthenticationTicket objects representing the user's identity if authentication is successful.
  - Return 'no result' or 'failure' if authentication is unsuccessful.
  - Have methods for challenge and forbid actions for when users attempt to access resources:
    - They're unauthorized to access (forbid).
    - When they're unauthenticated (challenge).

  Authenticate examples construct the user's identity with a JWT or cookie scheme

  Authorization invokes a challenge using the specified authentication scheme(s), or the default.
  This can include a cookie authentication scheme redirecting the user to a login page,
  or a JWT bearer scheme returning a 401 result with a "www-authenticate: bearer" header.

  Authentication forbid examples include:
  - A cookie authentication scheme redirecting the user to a page indicating access was forbidden.
  - A JWT bearer scheme returning a 403 result.
  - A custom authentication scheme redirecting to a page where the user can request access to the resource.

  -----------------------

  ASP.NET Core Identity:
  - Is an API that supports user interface (UI) login functionality.
  - Manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more.
  Users can create an account with the login information stored in Identity or they can use an external login provider.
  Identity is typically configured using a SQL Server database to store user names, passwords, and profile data
  */
  public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration tokenOptionsConfig)
  {
    services.Configure<TokenOptions>(tokenOptionsConfig);
    // adds the configuration bound to the TokenOptions object in the json file and makes it a singleton
    var tokenOptions = tokenOptionsConfig.Get<TokenOptions>();
    var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
    services.AddSingleton(signingConfigurations); // is necessary
    //authentication added in AuthBehavior
    services.AddAuthorization(opt =>
    {
      opt.AddPolicy(UserResourceRequirement.Policy, policy => policy.Requirements.Add(new UserResourceRequirement()));
      //"UserResource" is the policy scheme, used in [Authorize(Policy = "Admin")]
    });
  }
}
//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//  .AddJwtBearer(jwtBearerOptions =>
//  {
//    jwtBearerOptions.TokenValidationParameters =
//      new TokenValidationParameters()
//      {
//        ValidateAudience = true, // true by default
//        ValidateLifetime = true, // true by default
//        ValidateIssuer = true, // true by default
//        ValidateIssuerSigningKey = true, // true by default
//        ValidIssuer = tokenOptions.Issuer,
//        ValidAudience = tokenOptions.Audience,
//        IssuerSigningKey = signingConfigurations.SecurityKey,
//        ClockSkew = TimeSpan.Zero
//      };
//  });
//services.AddAuthorization(opt =>
//{
//  opt.AddPolicy(UserResourceRequirement.Policy, policy => policy.Requirements.Add(new UserResourceRequirement()));
//  //"UserResource" is the policy scheme, used in [Authorize(Policy = "Admin")]
//});
//services.AddSingleton<IAuthorizationHandler, UserResourceAuthorizationHandler>();
////services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();



//**
// Bearer in this case specifies the default scheme, when no other is provided
//builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
//{
//  opt.TokenValidationParameters = new TokenValidationParameters
//  {
//    ValidateIssuer = true, // default
//    ValidateAudience = true, // default
//    ValidateLifetime = true, // default
//    ValidateIssuerSigningKey = true, // not default
//    //LifetimeValidator = "asd", // used to override the default validation of the lifetime
//    ValidIssuer = builder.Configuration["Authentication:Issuer"], // allows only tokens generated by me
//    ValidAudience = builder.Configuration["Authentication:Audience"], // allows only tokens used by me
//    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"])),
//    ClockSkew = TimeSpan.Zero,
//  };
//});
//**