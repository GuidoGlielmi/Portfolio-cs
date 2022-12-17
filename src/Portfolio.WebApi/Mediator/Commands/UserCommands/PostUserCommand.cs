using MediatR;
using Portfolio.WebApi.DTO.UserDtos;
using Portfolio.WebApi.Models;

namespace Portfolio.WebApi.Mediator.Commands.UserCommands;

public record PostUserCommand(UserPostDto UserPostDto) : ICommand<UserPutDto> { }