using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Portfolio.WebApi.DTO;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Errors;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Controllers;

[Route("api/[controller]")] // implemented at controller level
// to provide a prefix to all templates defined at action level
[ApiController]
[Authorize(Roles = "ADMIN")]
public class UsersController : ControllerBase
{
  private readonly IService<User, UserSearcheable> _repo;

  private readonly PortfolioMapper<User, UserPostDto, UserPutDto> _mapper;

  public UsersController(IService<User, UserSearcheable> repo, PortfolioMapper<User, UserPostDto, UserPutDto> mapper)
  {
    _repo = repo;
    _mapper = mapper;
  }

  // GET: api/Users
  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<UserPutDto>>> GetUsers([FromQuery] UserSearcheable searchObj)
  {
    try
    {
      IEnumerable<User> users = await _repo.GetAll();
      if (ModelState.Count > 0)
      {
        users = _repo.Filter(users, searchObj);
      }
      return new ResponseDto<UserPutDto>(_mapper.ToPutDto(users));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // GET: api/Users/5
  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<ResponseDto<UserPutDto>>> GetUser(Guid id)
  {
    try
    {
      User user = await _repo.GetById(id);
      return new ResponseDto<UserPutDto>(_mapper.ToPutDto(user));
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // PUT: api/Users/5
  [HttpPut("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PutUser(UserPutDto userDto)
  {
    try
    {
      User user = _mapper.FromPutDto(userDto);
      await _repo.Update(user);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // POST: api/Users
  [HttpPost]
  public async Task<ActionResult<ResponseDto<UserPutDto>>> PostUser(UserPostDto userDto)
  {
    try
    {
      User user = _mapper.FromPostDto(userDto);
      await _repo.Create(user);
      var response = new ResponseDto<UserPutDto>(_mapper.ToPutDto(user));
      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  // DELETE: api/Users/5
  [HttpDelete("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> DeleteUser(Guid id)
  {
    try
    {
      User user = await _repo.GetById(id);
      await _repo.Delete(user);
      return new ResponseDto<string>(201);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<ResponseDto<string>>> PatchUser(Guid id, JsonPatchDocument<User> patchDocument)
  {
    User foundUser;
    try
    {
      foundUser = await _repo.GetById(id);
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }

    patchDocument.ApplyTo(foundUser, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(foundUser))
    {
      IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
      return StatusCode(400, new RequestException(400, errors).Error);
    }

    try
    {
      await _repo.Update(foundUser);
      return new ResponseDto<string>();
    } catch (RequestException ex)
    {
      return StatusCode(ex.Error.Code, ex.Error);
    }
  }
}
