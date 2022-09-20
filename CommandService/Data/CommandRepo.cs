using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        public void CreateCommand(int platId, Command command)
        {
            if (command == null) 
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null) {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                    .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                    .Where(c => c.PlatformId == platformId)
                    .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public CommandRepo(AppDbContext contet)
        {
            _context = contet;
        }
        private AppDbContext _context;
    }
}