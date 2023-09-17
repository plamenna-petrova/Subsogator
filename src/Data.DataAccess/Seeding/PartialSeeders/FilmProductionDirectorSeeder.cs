using Data.DataModels.Entities;

namespace Data.DataAccess.Seeding
{
    internal static class FilmProductionDirectorSeeder
    {
        internal static FilmProductionDirector[] FilmProductionDirectorSeedingArray { get; private set; }
            = SeedFilmProductionDirectors();

        private static FilmProductionDirector[] SeedFilmProductionDirectors()
        {
            var filmProductionDirectosToSeed = new FilmProductionDirector[]
            {
                new FilmProductionDirector()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    DirectorId = DirectorSeeder.DirectorSeedingArray[0].Id
                },
                new FilmProductionDirector()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    DirectorId = DirectorSeeder.DirectorSeedingArray[1].Id
                },
                new FilmProductionDirector()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    DirectorId = DirectorSeeder.DirectorSeedingArray[2].Id
                },
                new FilmProductionDirector()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    DirectorId = DirectorSeeder.DirectorSeedingArray[0].Id
                },
                new FilmProductionDirector()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    DirectorId = DirectorSeeder.DirectorSeedingArray[0].Id
                }
            };

            return filmProductionDirectosToSeed;
        }
    }
}
