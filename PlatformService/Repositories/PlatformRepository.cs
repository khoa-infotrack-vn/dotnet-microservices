using System;
using System.Collections.Generic;
using System.Linq;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _appDbContext;

        public PlatformRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool SaveChanges()
        {
            return this._appDbContext.SaveChanges() >= 0;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return this._appDbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return this._appDbContext.Platforms.Find(id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            this._appDbContext.Platforms.Add(platform);
        }
    }
}
