
namespace Portfolio.WebApi.Models;

public class User
{
  public Guid Id { get; set; }

  public string AboutMe { get; set; }

  public string FirstName { get; set; }

  public string GithubUrl { get; set; }

  public string LastName { get; set; }

  public string LinkedInUrl { get; set; }

  public string Password { get; set; }

  public string ProfileImg { get; set; }

  public string Username { get; set; }

  public Role Role { get; set; } = new();

  public DateTime CreatedAt { get; set; }
}
