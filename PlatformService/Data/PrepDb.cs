using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext? context, bool isProd) 
        {
            if (context != null && !context.Platforms.Any()) 
            {
                System.Console.WriteLine("---> Seeding data...");
                
                if (isProd) 
                {
                    Console.WriteLine("--> Attempting to apply migrations");
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Could not run migration {ex.Message}");                    
                    }
                }
                else
                {
                    context.Platforms.AddRange(
                        new Models.Platform() {Name = "DotNet",Publisher = "Microsoft",Cost = "Free"},
                    new Models.Platform() {Name = "SQL Server Express",Publisher = "Microsoft",Cost = "Free"},
                    new Models.Platform() {Name = "Kubernetes",Publisher = "Cloud Native Computing Foundation",Cost = "Free"});
                }
                context.SaveChanges();
            }
            else {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}