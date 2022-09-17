using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        [HttpPost]
        public ActionResult TestInboundConnection() 
        {
            _logger.LogInformation("Inbound POST # Command Service");
            return Ok("Inbound test ok from platforms controller");
        }

        public PlatformsController(ILogger<PlatformsController> logger)
        {
            _logger = logger;
        }

        private ILogger<PlatformsController> _logger;
    }
}