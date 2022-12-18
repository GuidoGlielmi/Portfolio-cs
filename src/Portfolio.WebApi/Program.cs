// dotnet tool install --global dotnet-ef
// dotnet ef dbcontext scaffold "Host=localhost;Database=portfolio;Username=postgres;Password=jorgedro" Npgsql.EntityFrameworkCore.PostgreSQL -o Models

// You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148
// dotnet ef dbcontext scaffold Name=ConnectionStrings:portfolioDB Npgsql.EntityFrameworkCore.PostgreSQL -o Models

// web servers typically log requests URL's but not request bodies, that's why is better
// to include a username and password in the request body.
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Filters;
using Portfolio.WebApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TokenOptions = Portfolio.WebApi.Security.Token.TokenOptions;

//}
//Log.Logger = new LoggerConfiguration()
//  .MinimumLevel.Debug()
//  .WriteTo.Console()
//  .WriteTo.File("logs/protfolio.txt", rollingInterval: RollingInterval.Day)
//  .CreateLogger();

// IOC containers (DI containers or service containers) are used to instantiate types and their dependencies
// they are able to resolve the dependencies of classes at runtime
// ASP.NET uses builder.SERVICES
// Autofac can be used as well
var asd = Assembly.GetExecutingAssembly().FullName;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// this already registers an array of logging providers:
// - Console
// - Debug
// - EventSource
// - EventLog: Windows only


////////////////////
// to add a custom logger:
//builder.Logging.ClearProviders(); // removes all loggin providers
//builder.Logging.AddConsole();
//builder.Host.UseSerilog();
////////////////////

//To determine the runtime environment, ASP.NET Core reads from the following environment variables:
// - DOTNET_ENVIRONMENT
// - ASPNETCORE_ENVIRONMENT when the WebApplication.CreateBuilder method is called. Overrides DOTNET_ENVIRONMENT.

//builder.Configuration.AddEnvironmentVariables(prefix: "MyCustomPrefix_");
// The default configuration loads environment variables and command line arguments prefixed with
// DOTNET_ and ASPNETCORE_


//builder.WebHost.UseUrls("https://localhost:3000"); // url used for the server


builder.Services.AddCors(opt =>
{
  // CORS is applied to requests when an Origin header is included in the request.
  // This includes requests made from JavaScript and POST requests.

  // browsers enforce a same-origin policy,
  // which means web pages cannot make requests in their javascript to a different domain than the one that served it.
  // So, if I load an SPA from Vercel, firefox won't allow me to do post requests to ANY
  // other server than that.
  // An HTTP client other than a browser won't use either the same origin policy or CORS, like on Postman 

  // CORS and the same origin policy are needed
  // because a browser does not implicitly trust the websites it visits to make requests to other websites.
  // in this case the Vercel loaded app to make requests to any other domain, even its own server.

  // They don't protect the origin site (Vercel app),
  // they protect the site receiving the cross origin requests (any targeted site by the Vercel app)
  // This is why the allowed origins are up to the targeted server

  // two urls have the same origin if the have the same scheme, host and port.
  // cors allows servers to be accessed by other domains.
  // So when a client sends an OPTION pre-flight request, the server sends back 
  // a series of headers indicating the restrictions it has to be accessed (headers, origins and methods)
  opt.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins("http://localhost:3000");
    policy.WithMethods("GET", "POST", "PUT", "DELETE", "PATCH");
    // policy.AllowAnyHeader();
    // policy.WithExposedHeaders(); // let client script access custom response header returned from cross-origin requests.
    // basically, any custom header returned from here
  });
});

// - AddTransient: Transient lifetime services are created each time they are requested.
//     This lifetime works best for lightweight, stateless services.
// - AddScoped: Scoped lifetime services are created once per request.


builder.Services.AddPortfolioServices();


//The various options interfaces exposed in .NET enables mapping configuration settings
//to strongly typed classes that can be accessed across various service lifetimes.
//basically it binds a class and its properties to an object found in a config json file
IConfiguration tokenOptionsConfig = builder.Configuration.GetSection(nameof(TokenOptions));
builder.Services.ConfigureJwtAuthentication(tokenOptionsConfig);

// Add services to the container.
builder.Services.AddControllers(opt =>
  {
    opt.ReturnHttpNotAcceptable = true;
    opt.Filters.Add(new ProducesAttribute("application/json", "text/html"));
    opt.Filters.Add(new ConsumesAttribute("application/json"));
    opt.Filters.Add<ExceptionFilter>(); // the IHostEnvironment injection is supported out of the box
    //opt.Filters.Add<SearchFilter>();
    //opt.Filters.Add<ResultFilter>();
    var asd = opt.ModelBinderProviders;
    // there are a lot of model binder providers
    // being ComplexObjectModelBinderProvider among them.
    opt.ReturnHttpNotAcceptable = true;

    //opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    // all non-nullable will be treated as required

    opt.ValidateComplexTypesIfChildValidationFails = false;
    //opt.ModelBinderProviders.Insert(0, snew CustomModelBinderProvider());
    //When evaluating model binders, the collection of providers is examined in order.
    //The first provider that returns a binder that matches the input model is used.
    // Model binders shouldn't attempt to set status codes or return results 
  })
  .AddXmlDataContractSerializerFormatters()
  .AddJsonOptions(x =>
  {
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  })
  .AddNewtonsoftJson(opt =>
  {
    opt.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
  });

builder.Services.AddControllersWithViews();


var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddMediatR(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);
ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.AddAutoMapper(assembly); // get the profiles in this assembly
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// To use the Secret Manager tool:
// dotnet user-secrets set "Database:Password" "password".
// To open the secrets, right click on the project and select Manage User Secrets
//string connString2 = Environment.GetEnvironmentVariable("PORTFOLIOCS_CONN_STR");

//string connString = builder.Configuration.GetConnectionString("PortfolioDb");
// the connection string is searched for in several places:
// - appsettings.Development.json
// - appsettings.Production.json
// - environment variables
// a local environment overrides any other so using one would mean the same value
// for Development and Production

string portfolioDatabaseConnectionString;

if (Environment.GetEnvironmentVariable("IS_LOCAL") == "TRUE")
{
  portfolioDatabaseConnectionString = builder.Configuration["Database:ConnectionStringPostgres"];
  builder.Services.AddDbContext<PortfolioContext>(opt => opt.UseNpgsql(portfolioDatabaseConnectionString));
} else
{
  portfolioDatabaseConnectionString = builder.Configuration["Database:ConnectionStringAzureSql"];
  builder.Services.AddDbContext<PortfolioContext>(opt => opt.UseSqlServer(portfolioDatabaseConnectionString));
}

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
  // disable automatic 400 responses
  opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMvc(options =>
{
  // adds custom 400 error response
  options.Filters.Add<BadRequestResponseActionFilter>();
  // necessary even using fluentValidation, because, for example, it checks for type conversion errors.
  // That's useful because in said case, the model will be set entirely to null instead,
  // and fluentValidation can't handle that
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

builder.Services.AddRazorPages()
  .AddMvcOptions(opt =>
  {
    opt.ModelMetadataDetailsProviders.Add(
      new ExcludeBindingMetadataProvider(typeof(Education)));
  });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // access ASPNETCORE_ENVIRONMENT env variable
{
  app.UseSwagger();
  app.UseSwaggerUI();
} else
{
  //app.UseExceptionHandler("/Error");// adds a middleware to the pipeline that catches errors
  app.UseHsts(); // HTTP Strict Transport Security.
  // Sends the homonym response header indicating browsers should use HTTPS, which relies on client implementation
}

app.UseHttpsRedirection(); // avoids responding to http requests (which may contain interceptable sensitive data)
app.UseStatusCodePagesWithRedirects("/error");
app.UseStaticFiles();

app.UseCors(); // should be before authentication and after routing

//app.UseRouting() // sets the position in the middleware pipeline where a routing decision is made

app.UseAuthentication();
app.UseAuthorization(); // usually put between between UseRouting and UseEndpoints

// the endpoint mapping can be done attribute-based (used for API's) or convention-based

//app.UseEndpoints(endpoints => endpoints.MapControllers()) ;
//app.UseEndpoint() // sets the position in the middleware pipeline where the selected endpoint is executed

app.MapControllers(); // adds enpoints for our controller's actions,
// in this case without specifying routes (done through attributes)

//Apps typically don't need to call UseRouting or UseEndpoints.
//WebApplicationBuilder configures a middleware pipeline that wraps middleware added in Program.cs with UseRouting and UseEndpoints.
//However, apps can change the order in which UseRouting and UseEndpoints run by calling these methods explicitly.


using (var context = new PortfolioContext(portfolioDatabaseConnectionString))
{
  context.Database.EnsureDeleted();
  context.Database.EnsureCreated();

  var adminRole = new Role
  {
    RoleName = Role.Roles.ADMIN,
  };

  var user = new User
  {
    FirstName = "Guido",
    LastName = "Glielmi",
    Username = "Guido",
    AboutMe = "kdsflsdkmflsdkmflsdkfmlsdkfmñsdlf,ñsdlf,",
    GithubUrl = "www.asd.com",
    LinkedInUrl = "www.asd.com",
    ProfileImg = "./assets/logos/asd.asd",
    Role = adminRole
  };
  user.Password = new PasswordHasher<User>().HashPassword(user, "guido");
  var ed = new Education
  {
    Degree = "tuviejatuvieja",
    EducationImg = "./assets/logos/slkdfn.png",
    School = "tuviejatuvieja",
    User = user,
    StartDate = DateTime.UtcNow.ToString("MM/yyyy")
  };

  var ex = new Experience
  {
    Title = "tuviejatuvieja",
    ExperienceImg = "./assets/logos/slkdfn.png",
    Description = "tuviejatuviejatuviejatuviejatuvieja",
    Certificate = "./assets/img/certificates/asd.asd",
    User = user,
    StartDate = DateTime.UtcNow.ToString("MM/yyyy")
  };

  var skill = new Skill
  {
    Name = "tuvieja",
    AbilityPercentage = 9,
    Type = Skill.SkillTypes.LANGUAGE
  };

  var pUrl = new ProjectUrl
  {
    Name = "tuvieja",
    Url = "www.asd.com"
  };

  var pUrl2 = new ProjectUrl
  {
    Name = "projecturl2",
    Url = "www.projecturl2.com"
  };

  var project = new Project
  {
    Title = "project",
    Description = "a project",
    ProjectImg = "./assets/logos/slkdfn.png",
    DeployUrl = "www.asd.com",
    User = user,
  };
  project.Urls.Add(pUrl);

  var project2 = new Project
  {
    Title = "project2",
    Description = "a project2",
    ProjectImg = "./assets/logos/slkdfn.png",
    DeployUrl = "www.asd.com",
    User = user,
  };
  project2.Urls.Add(pUrl2);

  var tech = new Technology
  {
    Name = "tuvieja",
    TechImg = "./assets/logos/slkdfn.png",
  };
  tech.Projects.AddRange(new List<Project> { project, project2 });

  context.Roles.Add(adminRole);
  context.Users.Add(user);
  context.Educations.Add(ed);
  context.Experiences.Add(ex);
  context.Skills.Add(skill);
  context.Projects.AddRange(new Project[] { project, project2 });
  context.Technologies.Add(tech);

  context.SaveChanges();
}

app.Run();



// -----

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddSingleton<IAuthenticationSchemeProvider, CustomAuthenticationSchemeProvider>();
//builder.Services.AddDefaultIdentity<IdentityUser>(opt => opt.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>();

// -----

//Assembly.GetExecutingAssembly()
//    .GetTypes()
//    .Where(t =>
//      t.BaseType != null &&
//      t.IsGenericType &&
//      t.GetGenericTypeDefinition() == typeof(PortfolioMapper<,,>))
//    .ToList()
//    .ForEach(t => builder.Services.AddScoped(t, t.GetGenericTypeDefinition()));


// -----

//options.Filters.Add(new UnauthorizedResponseActionFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));


//HttpContext httpContext = context.HttpContext;
//RouteData routeData = httpContext.GetRouteData();
//var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
//await result.ExecuteResultAsync(actionContext);

//OnForbidden = c =>
//{
//  c.NoResult();
//  c.Response.StatusCode = 403;
//  c.Response.ContentType = "application/json";
//  var responseObj = new ResponseDto<string>(403, "Forbidden");
//  var result = new JsonResult(responseObj)
//  {
//    StatusCode = 403
//  };
//  c.Response.WriteAsJsonAsync(responseObj).Wait();
//  return Task.CompletedTask;
//},
//OnAuthenticationFailed = c =>
//{
//  c.NoResult();
//  c.Response.StatusCode = 401;
//  c.Response.ContentType = "application/json";
//  var responseObj = new ResponseDto<string>(403, "Unauthorized access");
//  var result = new JsonResult(responseObj)
//  {
//    StatusCode = 401
//  };
//  c.Response.WriteAsJsonAsync(responseObj).Wait();
//  return Task.CompletedTask;
//},
//opt.Events = new JwtBearerEvents
//{
//  OnChallenge = async context =>
//  {
//    // OnChallenge is not called on 403
//    //Call this to skip the default logic and avoid using the default response
//    context.HandleResponse();

//    context.HttpContext.Response.StatusCode = 401;
//    var responseObj = new ResponseDto<string>(401, "Unauthorized access");
//    await context.HttpContext.Response.WriteAsJsonAsync(responseObj);
//  }
//};


//opt =>
//{
//  opt.AddPolicy("asd", p =>
//  {
//    p.RequireAssertion(context =>
//    {
//      var filterContext = (AuthorizationFilterContext)context.Resource;
//      var httpMethod = filterContext.HttpContext.Request.Method;
//      return httpMethod != "GET";
//    });
//  });
//  opt.AddPolicy("Admin", policy =>
//  {
//    policy.RequireAuthenticatedUser();
//    policy.RequireRole(Role.Roles.ADMIN.ToString());
//    // when using conventional (standard) claim types like "given_name", the enum ClaimTypes MUST be used
//    //policy.RequireClaim(ClaimTypes.GivenName, "Guido");
//  });
//}

//builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(OptionsManager<>)));

//app.Use(async (context, next) =>
//{
//  await next();
//  // this prevents the response to be sent
//  // until the code below is executed

//  int unauthorizedStatusCode = (int)HttpStatusCode.Unauthorized;
//  if (context.Response.StatusCode == unauthorizedStatusCode)
//  {
//    var responseObj = new ResponseDto<string>(unauthorizedStatusCode, "Must be logged in");
//    await context.Response.WriteAsJsonAsync(responseObj);
//    return;
//  }

//  int forbiddenStatusCode = (int)HttpStatusCode.Forbidden;
//  if (context.Response.StatusCode == forbiddenStatusCode)
//  {
//    var responseObj = new ResponseDto<string>(forbiddenStatusCode, "Not allowed to perform that action");
//    await context.Response.WriteAsJsonAsync(responseObj);
//    return;
//  }
//});