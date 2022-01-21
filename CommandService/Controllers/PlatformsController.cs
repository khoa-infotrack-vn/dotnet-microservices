using System;
using System.Collections.Generic;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms from CommandService.");
            var platforms = _commandRepository.GetAllPlatforms();
            var response = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service.");
            return Ok("Inbound test ok from Platforms Controller");
        }
    }
}
