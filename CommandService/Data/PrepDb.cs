using CommandService.Models;
using CommandService.SynchDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder builder) {
            using (var serviceScope = builder.ApplicationServices.CreateScope()) {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient?.ReturnAllPlatforms();
                var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>();

                SeedData(commandRepo, platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms) {
            System.Console.WriteLine("Seeding new platforms...");
            if (platforms is null) {
                return;
            }

            foreach (var platform in platforms) {
                Console.WriteLine($"-------> platform: {platform.Name}, {platform.Id}");
                if (!repo.ExternalPlatformExists(platform.ExternalId)) {
                    repo.CreatePlatform(platform);
                }
            }
            repo.SaveChanges();
        }
    }
}