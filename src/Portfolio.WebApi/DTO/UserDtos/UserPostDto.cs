using Portfolio.WebApi.Validations.CustomAttributes;
using Portfolio.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.UserDtos;

public class UserPostDto
{

  [Required]
  [StringLength(50, MinimumLength = 5)]
  public string Username { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 5)]
  public string Password { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  [StringLength(300, MinimumLength = 20)]
  public string AboutMe { get; set; }

  [Required]
  [RegularExpression(@"^(https:\/\/)?(www\.)?github\.com\/\w+$", ErrorMessage = "Invalid url")]
  public string GithubUrl { get; set; }


  [Required]
  [RegularExpression(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/\w+$", ErrorMessage = "Invalid url")]
  public string LinkedInUrl { get; set; }

  [Required]
  [ImagePathValidator]
  public string ProfileImg { get; set; }

  public Role Role { get; set; } = new();
}
