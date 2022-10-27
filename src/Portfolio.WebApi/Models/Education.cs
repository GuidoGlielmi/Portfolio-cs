//using Microsoft.Build.Framework; // THIS IS NOT THE CORRECT IMPORT FOR DATA ANNOTATIONS
using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Mapper;
using System.ComponentModel.DataAnnotations; // THIS IS

namespace Portfolio.WebApi.Models;

public class Education : UserResource,
  IMapFrom<EducationPostDto>,
  IMapFrom<EducationPutDto>,
  IMapFrom<IEnumerable<EducationPostDto>>,
  IMapFrom<IEnumerable<EducationPutDto>>
{
  public Guid Id { get; set; }

  public string Degree { get; set; }

  public string EducationImg { get; set; }

  public string School { get; set; }

  public string StartDate { get; private set; }

  public void SetStartDate(string startDate)
  {
    StartDate = DateTime.Parse(startDate).ToString("MM/yyyy");
  }

  public string EndDate { get; private set; } = "Current";

  public void SetEndDate(string endDate)
  {
    StartDate = endDate.ToLower() == "current" ? "Current" : DateTime.Parse(endDate).ToString("MM / yyyy");
  }

  public Education Copy(Education education)
  {
    return new Education
    {
      Id = education.Id,
      CreatedAt = education.CreatedAt,
      Degree = education.Degree,
      EducationImg = education.EducationImg,
      EndDate = education.EndDate,
      School = education.School,
      StartDate = education.StartDate,
      User = education.User
    };
  }
}
