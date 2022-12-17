using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Portfolio.WebApi.Errors;

namespace Portfolio.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
  private readonly IHostEnvironment _hostEnvironment;

  public ExceptionFilter(IHostEnvironment hostEnvironment)
  {
    _hostEnvironment = hostEnvironment;
  }

  public void OnException(ExceptionContext context)
  {
    // if model is not valid this is not executed
    if (/*!_hostEnvironment.IsDevelopment() ||*/ context.ExceptionHandled)
    {
      return;
    }
    if (context.Exception is RequestException ex)
    {
      context.HttpContext.Response.StatusCode = ex.Code;
      context.Result = new ContentResult
      {
        ContentType = "application/json",
        Content = JsonConvert.SerializeObject(ex.ErrorMessages)
      };
    } else
    {
      context.Result = new ContentResult { Content = context.Exception.Message };
    }
  }
}