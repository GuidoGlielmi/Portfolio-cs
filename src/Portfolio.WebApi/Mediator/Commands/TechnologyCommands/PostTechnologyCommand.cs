using Portfolio.WebApi.DTO.TechnologyDtos;

namespace Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

public record PostTechnologyCommand(TechnologyPostDto TechnologyPostDto) : ICommand<TechnologyPutDto> { }