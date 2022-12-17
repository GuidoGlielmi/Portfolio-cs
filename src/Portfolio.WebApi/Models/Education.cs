using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Mapper;
//using Microsoft.Build.Framework; // THIS IS NOT THE CORRECT IMPORT FOR DATA ANNOTATIONS
using System.ComponentModel.DataAnnotations; // THIS IS

namespace Portfolio.WebApi.Models;

public class Education :
  IMapFrom<EducationPostDto>,
  IMapFrom<EducationPutDto>,
  IMapFrom<IEnumerable<EducationPostDto>>,
  IMapFrom<IEnumerable<EducationPutDto>>
{

  public Guid Id { get; set; }

  public User User { get; set; }

  public Guid UserId { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public string Degree { get; set; }

  public string EducationImg { get; set; }

  public string School { get; set; }

  public string StartDate { get; set; }


  // the model type checkings and validations are done in the dto's
  // the validations are done through validation contexts, handled by either
  // asp.net or fluentValidation
  // even if a dto is created with invalid values, the context will hold validation errors
  // which will by handled by the BadRequestActionFilter
  //public void SetStartDate(string startDate)
  //{
  //  StartDate = DateTime.Parse(startDate).ToString("MM/yyyy");
  //}

  public string EndDate { get; set; } = "Current";

  //public void SetEndDate(string endDate)
  //{
  //  StartDate = endDate.ToLower() == "current" ? "Current" : DateTime.Parse(endDate).ToString("MM / yyyy");
  //}
}
