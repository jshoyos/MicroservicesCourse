using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            _logger.LogInformation("Getting Platforms from CommandsService");
            var platformItems = _commandRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection() 
        {
            _logger.LogInformation("Inbound POST # Command Service");
            return Ok("Inbound test ok from platforms controller");
        }

        public PlatformsController(ILogger<PlatformsController> logger, ICommandRepo commandRepo, IMapper mapper)
        {
            _mapper = mapper;
            _commandRepo = commandRepo;
            _logger = logger;
        }

        private readonly ILogger<PlatformsController> _logger;
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
    }
}