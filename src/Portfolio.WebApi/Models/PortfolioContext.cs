using Microsoft.EntityFrameworkCore;
using Portfolio.WebApi.DTO;

namespace Portfolio.WebApi.Models;

public /*partial*/ class PortfolioContext : DbContext
{
  private readonly string connString;
  public PortfolioContext() { }
  public PortfolioContext(string connString)
  {
    this.connString = connString;
  }

  public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options) { }

  public DbSet<Education> Educations { get; set; }
  public DbSet<Experience> Experiences { get; set; }
  public DbSet<Project> Projects { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<Skill> Skills { get; set; }
  public DbSet<Technology> Technologies { get; set; }
  public DbSet<User> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      //To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
      //optionsBuilder.UseNpgsql("Host=localhost;Database=portfoliocs;Username=postgres;Password=jorgedro");
      // Install-Package System.Configuration.ConfigurationManager
      optionsBuilder.UseNpgsql(connString);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Education>(entity =>
    {
      entity.ToTable("education");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .HasColumnName("degree");

      entity.Property(e => e.EducationImg)
                .HasMaxLength(255)
                .HasColumnName("education_img");

      entity.Property(e => e.EndDate)
                .HasMaxLength(255)
                .HasColumnName("end_date");

      entity.Property(e => e.School)
                .HasMaxLength(255)
                .HasColumnName("school");

      entity.Property(e => e.StartDate)
                .HasMaxLength(255)
                .HasColumnName("start_date");
    });
    //var ed = new Education
    //{
    //  Degree = "tuvieja",
    //  EducationImg = "./assets/logos/slkdfn.png",
    //  School = "tuvieja",
    //  StartDate = DateTime.UtcNow,
    //};
    //ed.SetEndDate(DateTime.UtcNow);
    //modelBuilder.Entity<Education>().HasData(ed);

    modelBuilder.Entity<Experience>(entity =>
    {
      entity.ToTable("experiences");

      entity.Property(e => e.Id)
                .HasColumnName("id");

      entity.Property(e => e.Certificate)
                .HasMaxLength(255)
                .HasColumnName("certificate");

      entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");

      entity.Property(e => e.EndDate)
                .HasMaxLength(255)
                .HasColumnName("end_date");

      entity.Property(e => e.ExperienceImg)
                .HasMaxLength(255)
                .HasColumnName("experience_img");

      entity.Property(e => e.StartDate)
                .HasMaxLength(255)
                .HasColumnName("start_date");

      entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
    });
    //var ex = new Experience
    //{
    //  Title = "tuvieja",
    //  ExperienceImg = "./assets/logos/slkdfn.png",
    //  Description = "tuvieja",
    //  StartDate = DateTime.UtcNow,
    //};
    //modelBuilder.Entity<Experience>().HasData(ex);

    modelBuilder.Entity<Skill>(entity =>
    {
      entity.ToTable("skills");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.AbilityPercentage).HasColumnName("ability_percentage");

      entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

      entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
    });

    //var skill = new Skill
    //{
    //  Name = "tuvieja",
    //  AbilityPercentage = 9,
    //  Type = SkillDto.SkillTypes.Language
    //};
    //modelBuilder.Entity<Skill>().HasData(skill);

    modelBuilder.Entity<Project>(entity =>
    {
      entity.ToTable("projects");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.DeployUrl)
                .HasMaxLength(255)
                .HasColumnName("deploy_url");

      entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");

      entity.Property(e => e.ProjectImg)
                .HasMaxLength(255)
                .HasColumnName("project_img");

      entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
    });

    //var pUrl = new ProjectUrl
    //{
    //  Name = "tuvieja",
    //  Url = "www.asd.com"
    //};

    //var project = new Project
    //{
    //  Title = "project",
    //  Description = "a project",
    //  ProjectImg = "./assets/logos/slkdfn.png",
    //  DeployUrl = "www.asd.com",
    //};
    //project.Urls.Add(pUrl);

    //var project2 = new Project
    //{
    //  Title = "project2",
    //  Description = "a project2",
    //  ProjectImg = "./assets/logos/slkdfn.png",
    //  DeployUrl = "www.asd.com",
    //};
    //project2.Urls.Add(pUrl);
    //modelBuilder.Entity<Project>().HasData(project, project2);

    modelBuilder.Entity<ProjectUrl>(entity =>
    {
      entity.ToTable("project_url");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

      entity.Property(e => e.ProjectId).HasColumnName("project_id");

      entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");

      entity.HasOne(d => d.Project)
                .WithMany(p => p.Urls)
                .HasForeignKey(d => d.ProjectId);
      //.HasConstraintName("fkqy6hxdth2tkobgtj9hm1gjvpj");
    });

    modelBuilder.Entity<Role>(entity =>
    {
      entity.ToTable("role");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
    });

    modelBuilder.Entity<Technology>(entity =>
    {
      entity.ToTable("technologies");

      entity.Property(e => e.Id)
                //.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

      entity.Property(e => e.TechImg)
                .HasMaxLength(255)
                .HasColumnName("tech_img");
    });

    //var tech = new Technology
    //{
    //  Name = "tuvieja",
    //  TechImg = "./assets/logos/slkdfn.png",
    //};
    //modelBuilder.Entity<Technology>().HasData(tech);

    modelBuilder.Entity<User>(entity =>
    {
      entity.ToTable("users");

      entity.Property(e => e.Id)
                ////.ValueGeneratedNever()
                .HasColumnName("id");

      entity.Property(e => e.AboutMe)
                .HasMaxLength(255)
                .HasColumnName("about_me");

      entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");

      entity.Property(e => e.GithubUrl)
                .HasMaxLength(255)
                .HasColumnName("github_url");

      entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");

      entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .HasColumnName("linked_in_url");

      entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

      entity.Property(e => e.ProfileImg)
                .HasMaxLength(255)
                .HasColumnName("profile_img");

      entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
    });

    modelBuilder
    .Entity<Project>()
    .HasMany(p => p.Techs)
    .WithMany(p => p.Projects)
    .UsingEntity<Dictionary<string, object>>(
      "techs_projects",
      j => j
          .HasOne<Technology>()
          .WithMany()
          .HasForeignKey("tech_id"),
      j => j
          .HasOne<Project>()
          .WithMany()
          .HasForeignKey("project_id")
      );

    //modelBuilder.HasSequence("technologies_id_seq");

    //OnModelCreatingPartial(modelBuilder);
  }

  //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
