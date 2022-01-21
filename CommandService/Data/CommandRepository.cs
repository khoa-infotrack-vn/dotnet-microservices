using System;
using System.Collections.Generic;
using System.Linq;
using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommandRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool SaveChanges()
        {
            return _appDbContext.SaveChanges() >= 0;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _appDbContext.Platforms.ToList();
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _appDbContext.Platforms.Add(platform);
        }

        public bool PlatformExists(int platformId)
        {
            return _appDbContext.Platforms.Any(p => p.Id == platformId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _appDbContext.Commands.Where(c => c.PlatformId == platformId);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _appDbContext.Commands.SingleOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            _appDbContext.Commands.Add(command);
        }
    }
}
