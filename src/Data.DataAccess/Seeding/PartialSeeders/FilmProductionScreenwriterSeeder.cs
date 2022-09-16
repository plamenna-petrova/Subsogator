using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class FilmProductionScreenwriterSeeder
    {
        internal static FilmProductionScreenwriter[] FilmProductionScreenwriterSeedingArray { get; private set; }
            = SeedFilmProductionScreenwriters();

        private static FilmProductionScreenwriter[] SeedFilmProductionScreenwriters()
        {
            var filmProductionScreenwritersToSeed = new FilmProductionScreenwriter[]
            {
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[0].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[1].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[2].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[3].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[4].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[5].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[0].Id
                },
                new FilmProductionScreenwriter()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    ScreenwriterId = ScreenwriterSeeder.ScreenwriterSeedingArray[6].Id
                }
            };

            return filmProductionScreenwritersToSeed;
        }
    }
}
