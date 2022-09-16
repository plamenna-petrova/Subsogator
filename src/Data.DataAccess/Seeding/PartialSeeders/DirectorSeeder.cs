using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class DirectorSeeder
    {
        internal static Director[] DirectorSeedingArray { get; private set; } = SeedDirectors();

        private static Director[] SeedDirectors() 
        {
            var directorsToSeed = new Director[]
            {
                new Director() 
                {
                    FirstName = "Christopher",
                    LastName = "Nolan",
                    CreatedOn = DateTime.UtcNow
                },
                new Director() 
                {
                    FirstName = "Paul",
                    LastName = "Greengrass",
                    CreatedOn = DateTime.UtcNow
                },
                new Director() 
                {
                    FirstName = "James",
                    LastName = "McTeigue",
                    CreatedOn = DateTime.UtcNow
                }
            };

            return directorsToSeed;
        }
    }
}
