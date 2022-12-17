using MediatR;
using Portfolio.WebApi.DTO.SkillDtos;

namespace Portfolio.WebApi.Mediator.Commands.SkillCommands;

public record PutSkillCommand(SkillPutDto SkillPutDto) : ICommand<Unit> { }