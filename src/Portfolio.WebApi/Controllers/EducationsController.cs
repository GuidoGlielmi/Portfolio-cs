using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;


// Controllers annotated with the [ApiController] attribute automatically validate model state and return a 400 response.
[Route("api/[controller]")]
[ApiController] // this infers [FromBody] for complex types
// infers [FromRoute] for any action parameter name matching a parameter in the route template
// [FromQuery] is inferred for any other action parameters
//[Authorize(Roles = "ADMIN")]
public class EducationsController : ControllerBase
{
  // if an unhandled exception ocurrs, the framework will automatically return a 500.

  private readonly IService<Education, EducationSearcheable> _repo;

  private readonly ILogger<EducationsController> _logger;

  private readonly PortfolioMapper<Education, EducationPostDto, EducationPutDto> _mapper;

  public EducationsController(IService<Education, EducationSearcheable> service, PortfolioMapper<Education, EducationPostDto, EducationPutDto> mapper, ILogger<EducationsController> logger)
  {
    _repo = service;
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    // the container may be other than the provided by ASP.NET, which may be null
    //HttpContext.RequestServices.GetService(typeof(ILogger<EducationsController>));
    _mapper = mapper;
  }

  // GET: api/Educations
  [AllowAnonymous]
  [HttpGet]
  public async Task<ActionResult<ResponseDto<EducationPutDto>>> GetEducations([FromQuery] EducationSearcheable searchObj)
  {
    //string requestingUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;
    // C# doesn't support implicit cast operators on interfaces.
    // Consequently, conversion of the interface to a concrete type is necessary to use ActionResult.
    try
    {
      IEnumerable<Education> educations = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        educations = _repo.Filter(educations, searchObj);
      }
      return new ResponseDto<EducationPutDto>(_mapper.ToPutDto(educations));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Educations/5
  [AllowAnonymous]
  [HttpGet("{id}")]
  public async Task<ActionResult<ResponseDto<EducationPutDto>>> GetEducation(Guid id)
  {
    try
    {
      Education education = await _repo.GetById(id);
      return new ResponseDto<EducationPutDto>(_mapper.ToPutDto(education));
    } catch (RequestException ex)
    {
      _logger.LogCritical($"The education with id:{id} was not found", ex);
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Educations/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut]
  public async Task<ActionResult<ResponseDto<string>>> PutEducation(EducationPutDto educationDto)
  {
    // !ModelState.IsValid is unnecessary since [ApiController] validation errors automatically trigger an HTTP 400 response
    try
    {
      Education education = _mapper.FromPutDto(educationDto);
      await _repo.Update(education);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Educations
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<ResponseDto<EducationPutDto>>> PostEducation(EducationPostDto educationDto)
  {
    try
    {
      Education education = _mapper.FromPostDto(educationDto);
      await _repo.Create(education);
      var response = new ResponseDto<EducationPutDto>(_mapper.ToPutDto(education));
      // CreatedAtAction generates the location for fetching the newly created data
      // GetEducation as actionName refers to the method, which is used for the route template
      return CreatedAtAction(nameof(GetEducation), new { id = education.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Educations/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteEducation(Guid id)
  {
    //string requestingUserId = User.Claims.FirstOrDefault(
    //  c => c.Type == ClaimTypes.PrimarySid
    //).Value;

    try
    {
      Education education = await _repo.GetById(id);
      //if (education.UserId.ToString() != requestingUserId)
      //{
      //  throw new RequestException(403);
      //}
      await _repo.Delete(education);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchEducation(Guid id, JsonPatchDocument<Education> patchDocument)
  {
    // a patch request should be handled through content negotiation,
    // which make it possible to serve different versions of a resource at the same URI
    // indicated by the user.
    /*
    A JsonPatchDocument has this structure:
    [
      {
        "op": <action-to-perform>, // add - remove - replace - move - copy - test
        "path": <prop-to-modify>,
        "value": <entered-value>
      }
    ]
    which is a list of object operations to perform on an entity
    */
    Education foundEd;
    try
    {
      foundEd = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundEd, ModelState);
    // ModelState is a JsonPatchDocument of type Education,
    // so as long a no unexistent properties are included, no errors will occur,
    // but no validation will be made.

    if (!ModelState.IsValid || !TryValidateModel(foundEd))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    await _repo.Update(foundEd);

    return new ResponseDto<string>();
  }
}