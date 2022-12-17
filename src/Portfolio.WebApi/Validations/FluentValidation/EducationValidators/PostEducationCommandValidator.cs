using FluentValidation;
using Portfolio.WebApi.Mediator.Commands.EducationCommands;

namespace Portfolio.WebApi.Validations.FluentValidation.EducationValidators;

public class PostEducationCommandValidator : AbstractValidator<PostEducationCommand>
{
	// called after CreateEducationCommand
	public PostEducationCommandValidator()
	{
		RuleFor(e => e.EducationPostDto.EndDate)
			.Must(endDate =>
				DateTime.TryParse(endDate, out DateTime _)
				|| endDate.ToLower() == "current")
			.WithMessage("Invalid EndDate");
		RuleFor(e => e.EducationPostDto.StartDate)
			.Must(startDate => DateTime.TryParse(startDate, out DateTime _))
			.WithMessage("Invalid StartDate");
		RuleFor(e => e.EducationPostDto.Degree)
			.MinimumLength(10);
		RuleFor(e => e.EducationPostDto.School)
			.MinimumLength(10);
		RuleFor(e => e.EducationPostDto.EducationImg)
			.Matches(@"^\./assets/logos/\w+\.[a-zA-Z]{2,5}$")
			.WithMessage("Incorrect image path");
	}
}
