using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace Portfolio.WebApi.Extensions;

public static class ConfigureSwaggerGenExtensions
{
  public static void ConfigureSwaggerGen(this IServiceCollection service)
  {
    service.AddSwaggerGen(setup =>
    {
      var jwtSecurityScheme = new OpenApiSecurityScheme
      {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
          Id = JwtBearerDefaults.AuthenticationScheme,
          Type = ReferenceType.SecurityScheme
        }
      };

      setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

      setup.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
          { jwtSecurityScheme, Array.Empty<string>() }
      });
    });
  }
}
