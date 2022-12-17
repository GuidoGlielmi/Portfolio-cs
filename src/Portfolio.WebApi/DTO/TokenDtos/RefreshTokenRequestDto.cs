using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.TokenDtos;

public class RefreshTokenRequestDto
{
  [Required]
  public string RefreshToken { get; set; }

  [Required]
  [DefaultValue("Guido")]
  public string UserName { get; set; }
}