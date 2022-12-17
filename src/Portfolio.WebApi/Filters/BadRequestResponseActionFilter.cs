using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portfolio.WebApi.Filters;

/*
 * The filter pipeline is executed once the action is selected, after the following pipeline:
 * Custom middlewares ->
 * Routing Middlewares -> 
 * Action Selection -> (filters)
*/
// Filter pipeline:
// Auth
// -> Resource Filters
// -> Model Binding
// -> Actions Filters (possible action execution and result conversion)
// -> Exceptions Filters
// -> Result Filters
// -> Result execution
// -> Result Filters
// -> Resource Filters
/*
An action filter is an attribute that you can apply to a controller action
-- or an entire controller -- that modifies the way in which the action is executed.

OutputCache(Duration=10) is used to cache a result of a request the the specified amount of time,
returning always that same result
Action filters contain logic that is executed before and after a controller action executes,
i.e OnActionExecuting and OnActionExecuted respectively.

An ActionFilter can:
- change the arguments passed into an action.
- change the result returned from the action.

ResultFilter:
runs code immediately before and after the execution of action results (what an action returns).
They run only when the action method has executed successfully.
They are useful for logic that must surround view or formatter execution.
*/
public class BadRequestResponseActionFilter : ActionFilterAttribute
{
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      context.HttpContext.Response.StatusCode = 400;
      context.Result = new JsonResult(
        context.ModelState.Values.SelectMany(ms =>
          ms.Errors.Select(e =>
            e.ErrorMessage)));
    }
  }
}
