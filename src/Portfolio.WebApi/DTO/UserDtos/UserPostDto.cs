using Portfolio.WebApi.DTO.RoleDtos;
using Portfolio.WebApi.Models;
using Portfolio.WebApi.Validations.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.UserDtos;

public class UserPostDto
{

  //[Required]
  //[StringLength(50, MinimumLength = 5)]
  public string Username { get; set; }

  //[Required]
  public string Password { get; set; }
  //[StringLength(50, MinimumLength = 5)]

  //[Required]
  public string FirstName { get; set; }

  //[Required]
  public string LastName { get; set; }

  //[Required]
  //[StringLength(300, MinimumLength = 20)]
  public string AboutMe { get; set; }

  //[Required]
  //[RegularExpression(@"^(https:\/\/)?(www\.)?github\.com\/\w+$", ErrorMessage = "Invalid url")]
  public string GithubUrl { get; set; }


  //[Required]
  //[RegularExpression(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/\w+$", ErrorMessage = "Invalid url")]
  public string LinkedInUrl { get; set; }

  //[Required]
  //[ImagePathValidator]
  public string ProfileImg { get; set; }

  public RolePostDto Role { get; set; } = new();
}
