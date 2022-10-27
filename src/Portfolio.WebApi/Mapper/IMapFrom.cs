using AutoMapper;

namespace Portfolio.WebApi.Mapper;

public interface IMapFrom<T>
{
  void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
  // T would be, for example, Education, and GetType() would get the class type that implements IMapFrom, which should be EducationPostDto and EducationPutDto
}