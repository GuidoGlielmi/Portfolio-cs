using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Mediator.Commands.UserCommands;
using Portfolio.WebApi.Mediator.Queries.UserQueries;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")] // implemented at controller level
// to provide a prefix to all templates defined at action level
[ApiController]
//[Authorize(Roles = "ADMIN")]
public class UsersController : ControllerBase
{
  private readonly IMediator _mediator;

  public UsersController(IMediator mediator)
  {
    _mediator = mediator;
  }

  // GET: api/Users
  [HttpGet]
  //[AllowAnonymous]
  public async Task<IEnumerable<UserPutDto>> GetUsers([FromQuery] UserSearcheable searchObj)
  {
    //IEnumerable<User> users = await _repo.GetAll();
    //if (ModelState.Count > 0)
    //{
    //  users = users.DynamicWhereAll(searchObj);
    //}
    //return _mapper.ToPutDto(users);
    return await _mediator.Send(new GetUsersQuery(searchObj.ToDictionary()));
  }

  // GET: api/Users/5
  [HttpGet("{id}")]
  //[AllowAnonymous]
  public async Task<UserPutDto> GetUser(Guid id)
  {
    //User user = await _repo.GetById(id);
    //return _mapper.ToPutDto(user);
    return await _mediator.Send(new GetUserByIdQuery(id));
  }

  // PUT: api/Users/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutUser(UserPutDto userDto)
  {
    //User user = _mapper.FromPutDto(userDto);
    //await _repo.Update(user);
    await _mediator.Send(new PutUserCommand(userDto));
    return Ok();
  }

  // POST: api/Users
  [HttpPost]
  public async Task<IActionResult> PostUser(UserPostDto userDto)
  {
    //User user = _mapper.FromPostDto(userDto);
    //await _repo.Create(user);
    //var response = _mapper.ToPutDto(user);
    var createdUser = await _mediator.Send(new PostUserCommand(userDto));
    return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);

  }

  // DELETE: api/Users/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteUser(Guid id)
  {
    //User user = await _repo.GetById(id);
    //await _repo.Delete(user);
    await _mediator.Send(new DeleteUserCommand(id));
    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchUser(Guid id, JsonPatchDocument<UserPutDto> patchDocument)
  {
    //User foundUser = await _repo.GetById(id);
    //patchDocument.ApplyTo(foundUser, ModelState);

    //if (!ModelState.IsValid || !TryValidateModel(foundUser))
    //{
    //  IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
    //  return StatusCode(400, errors);
    //}

    //await _mediator.Send(new PutUserCommand(foundUser));
    //await _repo.Update(foundUser);
    await _mediator.Send(new PatchUserCommand(id, patchDocument));
    return Ok();
  }
}
