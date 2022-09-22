using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
           _logger.LogInformation("Hit GetCommandsForPlatform: " + platformId);
           if (!_commandRepo.PlatformExists(platformId))
           {
                return NotFound();
           }
           
           var commands = _commandRepo.GetCommandsForPlatform(platformId);

           return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            _logger.LogInformation($"Hit GetCommandForPlatform {platformId}/{commandId}");
            if (!_commandRepo.PlatformExists(platformId)) return NotFound();

            var command = _commandRepo.GetCommand(platformId, commandId);

            if (command is null) return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto>  CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            _logger.LogInformation($"Hit CreateCommandForPlatform {platformId}");
            if (!_commandRepo.PlatformExists(platformId)) return NotFound();

            var command = _mapper.Map<Command>(commandDto);

            if (command is null) return BadRequest();

            _commandRepo.CreateCommand(platformId, command);
            _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
            new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }

        public CommandsController(ILogger<CommandsController> logger, ICommandRepo commandRepom, IMapper mapper)
        {
            _commandRepo = commandRepom;
            _mapper = mapper;
            _logger = logger;
            
        }
        private readonly ILogger<CommandsController> _logger;
        private readonly IMapper _mapper;
        private readonly ICommandRepo _commandRepo;
    }
}