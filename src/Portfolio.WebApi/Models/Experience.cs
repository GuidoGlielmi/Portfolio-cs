﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.WebApi.Models;

public class Experience : UserResource
{
  public Guid Id { get; set; }

  public string Title { get; set; }

  public string Certificate { get; set; }

  public string Description { get; set; }

  public string EndDate { get; private set; } = "Current";

  public string StartDate { get; private set; }

  public void SetStartDate(string startDate)
  {
    StartDate = DateTime.Parse(startDate).ToString("MM/yyyy");
  }

  public void SetEndDate(string endDate)
  {
    StartDate = endDate.ToLower() == "current" ? "Current" : DateTime.Parse(endDate).ToString("MM/yyyy");
  }

  public string ExperienceImg { get; set; }
}