using MediatR;
using Portfolio.WebApi.DTO.TechnologyDtos;

namespace Portfolio.WebApi.Mediator.Commands.TechnologyCommands;

public record PutTechnologyCommand(TechnologyPutDto TechnologyPutDto) : ICommand<Unit> { }
