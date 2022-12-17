using static Portfolio.WebApi.Models.Role;

namespace Portfolio.WebApi.DTO.RoleDtos;

public class RolePutDto
{
  public Guid Id { get; set; }

  public Roles RoleName { get; set; }

}
