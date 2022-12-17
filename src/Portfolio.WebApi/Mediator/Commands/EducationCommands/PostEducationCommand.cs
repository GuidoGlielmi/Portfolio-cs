using Portfolio.WebApi.DTO.EducationDtos;

namespace Portfolio.WebApi.Mediator.Commands.EducationCommands;

public record PostEducationCommand(EducationPostDto EducationPostDto) : ICommand<EducationPutDto> { }
