using CommandService.Models;
using AutoMapper;
using Grpc.Net.Client;

namespace CommandService.SynchDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        IEnumerable<Platform> IPlatformDataClient.ReturnAllPlatforms()
        {
            _logger.LogDebug($"Calling GRPC service: {_config["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
            var client = new PlatformService.GrpcPlatform.GrpcPlatformClient(channel);
            var request = new PlatformService.GetAllRequest();
            
            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception e)
            {
                _logger.LogError("could not call GRPC server: {0} ", e);
                return null;
            }
        }
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILogger<PlatformDataClient> _logger;

        public PlatformDataClient(IConfiguration config, IMapper mapper, ILogger<PlatformDataClient> logger)
        {
            _logger = logger;
            _config = config;
            _mapper = mapper;
            
        }
    }
}