namespace Portfolio.WebApi.DTO.EducationDtos;

public class EducationSearcheable
{
  public string Degree { get; set; }

  public string School { get; set; }

  public string StartDate { get; set; }

  public string EndDate { get; set; }
}

//public IEnumerable<Education> Filter(IEnumerable<Education> educations)
//{
//  if (!string.IsNullOrEmpty(Degree))
//  {
//    educations = educations.Where(e => e.Degree.Contains(Degree.Trim()));
//  }
//  if (!string.IsNullOrEmpty(School))
//  {
//    educations = educations.Where(e => e.School.Contains(School.Trim()));
//  }
//  if (!string.IsNullOrEmpty(StartDate))
//  {
//    educations = educations.Where(e => e.StartDate.Contains(StartDate.Trim()));
//  }
//  if (!string.IsNullOrEmpty(EndDate))
//  {
//    educations = educations.Where(e => e.EndDate.Contains(EndDate.Trim()));
//  }
//  return educations;
//}
