using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //source -> target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Platform, PlatformReadDto>();
        }
    }
}
