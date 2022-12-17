using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Portfolio.WebApi.DTO;

namespace Portfolio.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
  private readonly FileExtensionContentTypeProvider FECTP;

  public FilesController(FileExtensionContentTypeProvider fectp)
  {
    FECTP = fectp;
  }

  [HttpGet("{fileId}")]
  public IActionResult GetFile(string fileId)
  {
    string pathToFile = "stack_Spring.png";
    if (!System.IO.File.Exists(pathToFile))
    {
      return NotFound();
    }

    if (!FECTP.TryGetContentType(pathToFile, out string contentType))
    {
      contentType = "application/octet-stream"; // default value
    }

    byte[] bytes = System.IO.File.ReadAllBytes(pathToFile);

    return File(bytes, contentType, Path.GetFileName(pathToFile));
  }
}
