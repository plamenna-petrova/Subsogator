using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class FilmProductionGenreSeeder
    {
        internal static FilmProductionGenre[] FilmProductionGenreSeedingArray { get; private set; }
         = SeedFilmProductionGenres();

        private static FilmProductionGenre[] SeedFilmProductionGenres()
        {
            var filmProductionGenresToSeed = new FilmProductionGenre[]
            {
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[0].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[1].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[4].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[0].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[2].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[5].Id
                },
                new FilmProductionGenre() 
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[0].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[6].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[4].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[0].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[7].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[6].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[1].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[6].Id
                },
                new FilmProductionGenre()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    GenreId = GenreSeeder.GenreSeedingArray[4].Id
                }
            };

            return filmProductionGenresToSeed;
        }
    }
}
