using AutoMapper;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mapper.Implementations;

public class SkillMapper : IPortfolioMapper<Skill, SkillPostDto, SkillPutDto>
{
  private readonly IMapper Mapper;

  public SkillMapper(IMapper mapper)
  {
    Mapper = mapper;
  }
  public SkillPostDto ToPostDto(Skill entity) => Mapper.Map<SkillPostDto>(entity);

  public IEnumerable<SkillPostDto> ToPostDto(IEnumerable<Skill> entity) => entity.Select(e => Mapper.Map<SkillPostDto>(e));


  public Skill FromPostDto(SkillPostDto entity) => Mapper.Map<Skill>(entity);

  public IEnumerable<Skill> FromPostDto(IEnumerable<SkillPostDto> entity) => entity.Select(e => Mapper.Map<Skill>(e));


  public SkillPutDto ToPutDto(Skill entity) => Mapper.Map<SkillPutDto>(entity);

  public IEnumerable<SkillPutDto> ToPutDto(IEnumerable<Skill> entity) => entity.Select(e => Mapper.Map<SkillPutDto>(e));


  public Skill FromPutDto(SkillPutDto entity) => Mapper.Map<Skill>(entity);

  public IEnumerable<Skill> FromPutDto(IEnumerable<SkillPutDto> entity) => entity.Select(e => Mapper.Map<Skill>(e));

}
