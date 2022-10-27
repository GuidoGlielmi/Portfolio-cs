using AutoMapper;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;

public class UserMapper : IPortfolioMapper<User, UserPostDto, UserPutDto>
{
  private readonly IMapper Mapper;

  public UserMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public UserPostDto ToPostDto(User entity) => Mapper.Map<UserPostDto>(entity);

  public IEnumerable<UserPostDto> ToPostDto(IEnumerable<User> entity) => entity.Select(e => Mapper.Map<UserPostDto>(e));


  public User FromPostDto(UserPostDto entity) => Mapper.Map<User>(entity);

  public IEnumerable<User> FromPostDto(IEnumerable<UserPostDto> entity) => entity.Select(e => Mapper.Map<User>(e));


  public UserPutDto ToPutDto(User entity) => Mapper.Map<UserPutDto>(entity);

  public IEnumerable<UserPutDto> ToPutDto(IEnumerable<User> entity) => entity.Select(e => Mapper.Map<UserPutDto>(e));


  public User FromPutDto(UserPutDto entity) => Mapper.Map<User>(entity);

  public IEnumerable<User> FromPutDto(IEnumerable<UserPutDto> entity) => entity.Select(e => Mapper.Map<User>(e));

}
