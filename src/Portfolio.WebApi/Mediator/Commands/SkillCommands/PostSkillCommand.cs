using MediatR;
using Portfolio.WebApi.DTO.ExperienceDtos;
using Portfolio.WebApi.DTO.SkillDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.SkillCommands;

public record PostSkillCommand(SkillPostDto SkillPostDto) : ICommand<SkillPutDto> { }
