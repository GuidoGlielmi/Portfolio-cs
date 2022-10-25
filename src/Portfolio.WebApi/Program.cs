// dotnet tool install --global dotnet-ef
// dotnet ef dbcontext scaffold "Host=localhost;Database=portfolio;Username=postgres;Password=jorgedro" Npgsql.EntityFrameworkCore.PostgreSQL -o Models

// You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148
// dotnet ef dbcontext scaffold Name=ConnectionStrings:portfolioDB Npgsql.EntityFrameworkCore.PostgreSQL -o Models

// web servers typically log requests URL's but not request bodies, that's why is better
// to include a username and password in the request body.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.Models;
using Portfolio.WebApi.Repositories;
using System.Text;
using System.Text.Json.Serialization;

//Log.Logger = new LoggerConfiguration()
//  .MinimumLevel.Debug()
//  .WriteTo.Console()
//  .WriteTo.File("logs/protfolio.txt", rollingInterval: RollingInterval.Day)
//  .CreateLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args); // this already registers an array of logging providers.

//builder.Logging.ClearProviders(); // removes all loggin providers
//builder.Logging.AddConsole();
//builder.Host.UseSerilog();

// Bearer in this case specifies the default scheme, when no other is provided
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Authentication:Issuer"], // allows only tokens generated by me
    ValidAudience = builder.Configuration["Authentication:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"])),
  };
});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers(opt =>
  {
    opt.ReturnHttpNotAcceptable = true;
    opt.Filters.Add(new ProducesAttribute("application/json"));
    opt.Filters.Add(new ConsumesAttribute("application/json"));
  })
  .AddXmlDataContractSerializerFormatters()
  .AddJsonOptions(x =>
  {
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  })
  .AddNewtonsoftJson();

string connString = builder.Configuration.GetConnectionString("PortfolioDb");
// the connection string is searched for in several places:
// - appsettings.Development.json
// - appsettings.Production.json
// - environment variables
// a local environment overrides any other so using one would mean the same value
// for Development and Production
builder.Services.AddDbContext<PortfolioContext>(opt => opt.UseNpgsql(connString));

RepositoryConfiguration.AddRepositoryInjections(builder);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // get the profile in this assembly

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
  // disable automatic 400 responses
  opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMvc(options =>
{
  // adds custom 400 error response
  options.Filters.Add(new BadRequestResponseActionFilter());
});

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
//else
//{
//  app.UseExceptionHandler("/Error");
//  app.UseHsts();
//}

app.UseHttpsRedirection();

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


using (var context = new PortfolioContext(connString))
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
    Password = "guido",
    AboutMe = "kdsflsdkmflsdkmflsdkfmlsdkfm�sdlf,�sdlf,",
    GithubUrl = "www.asd.com",
    LinkedInUrl = "www.asd.com",
    ProfileImg = "./assets/logos/asd.asd",
    Role = adminRole
  };
  var ed = new Education
  {
    Degree = "tuvieja",
    EducationImg = "./assets/logos/slkdfn.png",
    School = "tuvieja",
    User = user,
  };
  ed.SetStartDate(DateTime.UtcNow.ToString("MM/yyyy"));

  var ex = new Experience
  {
    Title = "tuvieja",
    ExperienceImg = "./assets/logos/slkdfn.png",
    Description = "tuvieja",
    Certificate = "./assets/img/certificates/asd.asd",
    User = user,
  };
  ex.SetStartDate(DateTime.UtcNow.ToString("MM/yyyy"));

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