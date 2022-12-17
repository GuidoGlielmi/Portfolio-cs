using Portfolio.WebApi.DTO.ExperienceDtos;

namespace Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

public record PostExperienceCommand(ExperiencePostDto ExperiencePostDto) : ICommand<ExperiencePutDto> { }

