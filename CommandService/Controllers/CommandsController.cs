using System;
using System.Collections.Generic;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            var response = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(response);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _commandRepository.GetCommand(platformId, commandId);

            if (command is null)
            {
                return NotFound();
            }

            var response = _mapper.Map<CommandReadDto>(command);

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);
            _commandRepository.CreateCommand(platformId, command);
            _commandRepository.SaveChanges();

            var response = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(
                nameof(GetCommandForPlatform), 
                new {platformId = platformId, commandId = command.Id},
                response);
        }
    }
}
