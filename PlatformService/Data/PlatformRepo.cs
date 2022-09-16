using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        public void CreatePlatform(Platform platform)
        {
            if (platform is null) {
                throw new ArgumentNullException(nameof(platform));
            }
            _context.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int Id)
        {
            return _context.Platforms.FirstOrDefault(platform => platform.Id == Id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            this._context = context;
        }
    }
}