//using AutoMapper;
//using Portfolio.WebApi.DTO.EducationDtos;
//using Portfolio.WebApi.DTO.Experience;
//using Portfolio.WebApi.DTO.ExperienceDtos;
//using Portfolio.WebApi.DTO.ProjectDtos;
//using Portfolio.WebApi.DTO.ProjectUrlDtos;
//using Portfolio.WebApi.DTO.SkillDtos;
//using Portfolio.WebApi.DTO.TechnologyDtos;
//using Portfolio.WebApi.DTO.UserDtos;
//using Portfolio.WebApi.Models;

//namespace Portfolio.WebApi.Mapper.Profiles;

//public class PortfolioProfile : Profile
//{
//public PortfolioProfile()
//{
//  CreateMap<Education, EducationPostDto>().ReverseMap();
//  CreateMap<Education, EducationPutDto>().ReverseMap();
//  CreateMap<Education, EducationSearcheable>().ReverseMap();

//  CreateMap<Experience, ExperiencePostDto>().ReverseMap();
//  CreateMap<Experience, ExperiencePutDto>().ReverseMap();
//  CreateMap<Experience, ExperienceSearcheable>().ReverseMap();

//  CreateMap<Project, ProjectPostDto>().ReverseMap();
//  CreateMap<Project, ProjectPutDto>().ReverseMap();
//  CreateMap<Project, ProjectSearcheable>().ReverseMap();

//  CreateMap<ProjectUrl, ProjectUrlPostDto>().ReverseMap();
//  CreateMap<ProjectUrl, ProjectUrlPutDto>().ReverseMap();

//  CreateMap<Skill, SkillPostDto>().ReverseMap();
//  CreateMap<Skill, SkillPutDto>().ReverseMap();
//  CreateMap<Skill, SkillSearcheable>().ReverseMap();

//  CreateMap<Technology, TechnologyPostDto>().ReverseMap();
//  CreateMap<Technology, TechnologyPutDto>().ReverseMap();
//  CreateMap<Technology, TechnologyInProjectDto>().ReverseMap();
//  CreateMap<Technology, TechnologySearcheable>().ReverseMap();

//  CreateMap<User, UserPostDto>().ReverseMap();
//  CreateMap<User, UserPutDto>().ReverseMap();
//  CreateMap<User, UserSearcheable>().ReverseMap();
//  //CreateMap<Technology, TechnologyDto>().AfterMap((t, dto) => dto.ProjectsIds = (List<Guid>)t.Projects.Select(p => p.Id));
//}
//}
