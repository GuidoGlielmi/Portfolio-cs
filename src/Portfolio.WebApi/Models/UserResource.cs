namespace Portfolio.WebApi.Models;

public class UserResource
{
  public User User { get; set; }

  public Guid UserId { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
