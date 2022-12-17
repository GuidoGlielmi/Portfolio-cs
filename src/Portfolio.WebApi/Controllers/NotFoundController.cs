using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portfolio.WebApi.Controllers;

[Route("error")]
// the controllers name maps to the folder inside Views folder
public class NotFoundController : Controller
{
  [HttpGet]
  // the method name maps to the cshtml file inside the "NotFound[Controller]" folder inside View folder
  public IActionResult NotFoundPage()
  {
    return View();
  }
}
