using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class SubtitlesSeeder
    {
        internal static Subtitles[] SubtitlesSeedingArray { get; private set; } = SeedSubtitles();

        private static Subtitles[] SeedSubtitles()
        {
            var subtitlesToSeed = new Subtitles[]
            {
                new Subtitles()
                {
                    Name = "Inception Subs",
                    CreatedOn = DateTime.UtcNow,
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id
                },
                new Subtitles()
                {
                    Name = "The Bourne Supremacy Subs",
                    CreatedOn = DateTime.UtcNow,
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id
                },
                new Subtitles()
                {
                    Name = "V for Vendetta Subs",
                    CreatedOn = DateTime.UtcNow,
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id
                },
                new Subtitles()
                {
                    Name = "Batman Begins Subs",
                    CreatedOn = DateTime.UtcNow,
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id
                },
                new Subtitles()
                {
                    Name = "Interstellar Subs",
                    CreatedOn = DateTime.UtcNow,
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id
                }
            };

            return subtitlesToSeed;
        }
    }
}
