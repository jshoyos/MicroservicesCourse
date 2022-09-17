using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            _logger.LogInformation("---> Getting platforms...");
            
            var platforms = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }


        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id) 
        {
            _logger.LogInformation($"---> Getting platform: {id}");
            var platform = _platformRepo.GetPlatformById(id);
            if (platform is not null) {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }

            _logger.LogInformation($"platform: {id} not found");
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            _logger.LogInformation("Creating platform");
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _platformRepo.CreatePlatform(platform);
            _platformRepo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            try {
               await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
            }
            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }

        public PlatformsController(
            ILogger<PlatformsController> logger,
            IPlatformRepo platformRepo, IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _mapper = mapper;
            _platformRepo = platformRepo;
            _logger = logger;
            _commandDataClient = commandDataClient;
        }

        private readonly ILogger<PlatformsController> _logger;
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
    }
}