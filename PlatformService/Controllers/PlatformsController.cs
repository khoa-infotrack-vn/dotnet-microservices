using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepository platformRepository,
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms ....");
            var platforms = this._platformRepository.GetAllPlatforms();
            var responses = this._mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

            return Ok(responses);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting Platform ....");
            var platform = this._platformRepository.GetPlatformById(id);
            if (platform is null)
            {
                return NotFound();
            }
            var response = this._mapper.Map<PlatformReadDto>(platform);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = this._mapper.Map<Platform>(platformCreateDto);
            this._platformRepository.CreatePlatform(platform);
            this._platformRepository.SaveChanges();
            var platformReadDto = this._mapper.Map<PlatformReadDto>(platform);

            try
            {
                await _commandDataClient.SendPlatformToCommandAsync(platformReadDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
        }
    }
}
