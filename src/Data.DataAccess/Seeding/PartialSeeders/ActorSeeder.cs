using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class ActorSeeder
    {
        internal static Actor[] ActorSeedingArray { get; private set; } = SeedActors();

        private static Actor[] SeedActors() 
        {
            var actorsToSeed = new Actor[]
            {
                new Actor()
                {
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Joseph",
                    LastName = "Gordon-Levitt",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Elliot",
                    LastName = "Page",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Matt",
                    LastName = "Damon",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Franka",
                    LastName = "Potente",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Natalie",
                    LastName = "Portman",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Hugo",
                    LastName = "Weaving",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Christian",
                    LastName = "Bale",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Cillian",
                    LastName = "Murphy",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Matthew",
                    LastName = "McConaughey",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Anne",
                    LastName = "Hathaway",
                    CreatedOn = DateTime.UtcNow
                },
                new Actor()
                {
                    FirstName = "Jessica",
                    LastName = "Chastain",
                    CreatedOn = DateTime.UtcNow
                }
            };

            return actorsToSeed;
        }
    }
}
