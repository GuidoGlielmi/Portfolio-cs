using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Portfolio.WebApi.DTO;

namespace Portfolio.WebApi.Errors;

public class BadRequestResponseActionFilter : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      var errors = new List<string>();

      foreach (ModelStateEntry modelState in context.ModelState.Values)
      {
        foreach (ModelError error in modelState.Errors)
        {
          errors.Add(error.ErrorMessage);
        }
      }

      var responseObj = new ResponseDto<string>(400, errors);

      context.Result = new JsonResult(responseObj)
      {
        StatusCode = 400
      };
    }
  }

  public void OnActionExecuted(ActionExecutedContext context)
  { }
}
