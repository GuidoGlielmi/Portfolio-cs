
namespace Portfolio.WebApi.Models;

public class Skill
{
  public Guid Id { get; set; }

  public int AbilityPercentage { get; set; }

  public string Name { get; set; }

  public SkillTypes Type { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public enum SkillTypes
  {
    LANGUAGE,
    HARDANDSOFT
  }
}
