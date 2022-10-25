using Portfolio.WebApi.Validations.CustomAttributes;
using Portfolio.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.UserDtos;

public class UserPutDto
{
  public Guid Id { get; set; }

  [Required]
  [StringLength(300, MinimumLength = 20)]
  public string AboutMe { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  [RegularExpression(@"^(https:\/\/)?(www\.)?github\.com\/\w+$", ErrorMessage = "Invalid Github url")]
  public string GithubUrl { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  [RegularExpression(@"^(https:\/\/)?(www\.)?linkedin\.com\/in\/\w+$", ErrorMessage = "Invalid Linkedin url")]
  public string LinkedInUrl { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 5)]
  public string Password { get; set; }

  [Required]
  [ImagePathValidator]
  public string ProfileImg { get; set; }

  [Required]
  [StringLength(50, MinimumLength = 5)]
  public string Username { get; set; }

  public Role Role { get; set; } = new();

  public string FullName => $"{FirstName} {LastName}";

}
