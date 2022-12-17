using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.UserDtos;

public class UserCredentialsDto
{
  [Required]
  [StringLength(255)]
  [DefaultValue("Guido")]
  public string UserName { get; set; }

  [Required]
  [DefaultValue("guido")]
  [StringLength(32)]
  public string Password { get; set; }
}