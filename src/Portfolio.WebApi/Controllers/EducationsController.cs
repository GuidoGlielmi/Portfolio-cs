using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.AuthorizationRequirements;
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;
using Portfolio.WebApi.Mediator.Queries.EducationQueries;

namespace Portfolio.WebApi.Controllers;


// Controllers annotated with the [ApiController] attribute automatically validate model state and return a 400 response.
[Route("api/[controller]")]
[ApiController] // this infers [FromBody] for complex types
// infers [FromRoute] for any action parameter name matching a parameter in the route template
// [FromQuery] is inferred for any other action parameters
//[Authorize(Roles = "ADMIN", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
public class EducationsController : ControllerBase
{
  // if an unhandled exception ocurrs, the framework will automatically return a 500.

  private readonly ILogger<EducationsController> _logger;

  private readonly IMediator _mediator;
  //private readonly IAuthorizationService _authorizationService;
  public EducationsController(ILogger<EducationsController> logger, IMediator mediator)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    // the container may be other than the provided by ASP.NET, which may be null
    _mediator = mediator;
  }

  // GET: api/Educations
  [HttpGet]
  public async Task<IEnumerable<EducationPutDto>> GetEducations([FromQuery] EducationSearcheable searchObj)
  {
    return await _mediator.Send(new GetEducationsQuery(searchObj.ToDictionary()));

    //string requestingUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value;
    // C# doesn't support implicit cast operators on interfaces.
    // Consequently, conversion of the interface to a concrete type is necessary to use ActionResult.

    //IEnumerable<Education> educations = await _repo.GetAll();
    //if (ModelState.Count > 0)
    //{
    //educations = educations.DynamicWhereAll(searchObj);
    //}
    //return _mapper.ToPutDto(educations);
    // when returning a IEnumerable, there's not way to know which class it's going to be used
  }

  // GET: api/Educations/5
  [HttpGet("{id}")]
  [MinimumAgeAuthorize(21)]
  public async Task<ActionResult<EducationPutDto>> GetEducation(Guid id)
  {
    //Education createdEducation = await _repo.GetById(id);
    return await _mediator.Send(new GetEducationByIdQuery(id));
  }

  // PUT: api/Educations/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut]
  public async Task<IActionResult> PutEducation(EducationPutDto educationDto)
  {
    //var authorizationResult = await _authorizationService
    //      .AuthorizeAsync(User, educationDto, UserResourceRequirement.Policy);
    //if (authorizationResult.Succeeded)
    //{
    //}

    // !ModelState.IsValid is unnecessary since [ApiController] validation errors automatically trigger an HTTP 400 response
    //Education createdEducation = _mapper.FromPutDto(educationDto);
    //await _repo.Update(createdEducation);
    await _mediator.Send(new PutEducationCommand(educationDto));
    return Ok();
  }

  // POST: api/Educations
  [HttpPost]
  public async Task<ActionResult<EducationPutDto>> PostEducation(EducationPostDto educationDto)
  {
    // -> map incoming model to data model
    // -> use an abstraction to persist the model
    // -> map the data model back to the API model
    //Education createdEducation = _mapper.FromPostDto(educationDto);
    //await _repo.Create(createdEducation);
    var createdEducation = await _mediator.Send(new PostEducationCommand(educationDto));
    //var response = _mapper.ToPutDto(createdEducation);
    // CreatedAtAction generates the location for fetching the newly created data
    // GetEducation as actionName refers to the method, which is used for the route template
    return CreatedAtAction(nameof(GetEducation), new { id = createdEducation.Id }, createdEducation);
  }

  // DELETE: api/Educations/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteEducation(Guid id)
  {
    //string requestingUserId = User.Claims.FirstOrDefault(
    //  c => c.Type == ClaimTypes.PrimarySid
    //).Value;

    //Education education = await _repo.GetById(id);
    //if (createdEducation.UserId.ToString() != requestingUserId)
    //{
    //  throw new RequestException(403);
    //}
    //await _repo.Delete(education);
    await _mediator.Send(new DeleteEducationCommand(id));
    return Ok();
  }


  ///<remarks>
  /// {
  ///   "op": "replace",
  ///   "path": "/Degree",
  ///   "value": "tuviejatuvieja"
  ///  }
  /// </remarks>
  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchEducation(Guid id, JsonPatchDocument<EducationPutDto> patchDocument)
  {
    // ModelState is a JsonPatchDocument of type Education,
    // so as long a no unexistent properties are included, no errors will occur,
    // but no validation will be made.
    await _mediator.Send(new PatchEducationCommand(id, patchDocument));
    return Ok();
  }
}