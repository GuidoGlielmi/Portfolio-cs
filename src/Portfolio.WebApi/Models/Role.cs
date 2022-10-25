using System.Text.Json.Serialization;

namespace Portfolio.WebApi.Models;

public class Role
{
  public enum Roles
  {
    ADMIN,
    USER,
  }

  public Guid Id { get; set; }

  public Roles RoleName { get; set; }

  [JsonIgnore]
  public List<User> Users { get; set; }
}
