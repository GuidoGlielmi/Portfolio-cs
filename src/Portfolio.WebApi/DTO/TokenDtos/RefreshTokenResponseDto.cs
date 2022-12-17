using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Security.Token;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.WebApi.DTO.TokenDtos;

public class RefreshTokenResponseDto : IMapFrom<AccessToken>
{
  [Required]
  public string RefreshToken { get; set; }

  [Required]
  public DateTime Expiration { get; set; }

  [Required]
  public string Token { get; set; }
}