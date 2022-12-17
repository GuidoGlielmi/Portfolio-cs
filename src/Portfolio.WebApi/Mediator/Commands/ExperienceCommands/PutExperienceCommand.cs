using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;

namespace Portfolio.WebApi.Mediator.Commands.ExperienceCommands;

public record PutExperienceCommand(ExperiencePutDto ExperiencePutDto) : ICommand<Unit> { }
