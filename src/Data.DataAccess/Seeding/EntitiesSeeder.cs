using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    public class EntitiesSeeder : ISeeder
    {
        public async Task<bool> SeedDatabase(
            ApplicationDbContext applicationDbContext, 
            IServiceProvider serviceProvider
        )
        {
            if (applicationDbContext.Subtitles.Any())
            {
                return false;
            }

            await ExecutePartialSeeders(applicationDbContext);

            return true;
        }

        private async Task ExecutePartialSeeders(ApplicationDbContext applicationDbContext)
        {
            foreach (var actorToSeed in ActorSeeder.ActorSeedingArray)
            {
                await applicationDbContext.Actors.AddAsync(actorToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var countryToSeed in CountrySeeder.CountrySeedingArray)
            {
                await applicationDbContext.Countries.AddAsync(countryToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var directorToSeed in DirectorSeeder.DirectorSeedingArray)
            {
                await applicationDbContext .Directors.AddAsync(directorToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var genreToSeed in GenreSeeder.GenreSeedingArray)
            {
                await applicationDbContext.Genres.AddAsync(genreToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var languageToSeed in LanguageSeeder.LanguageSeedingArray)
            {
                await applicationDbContext.Languages.AddAsync(languageToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var screenwriterToSeed in ScreenwriterSeeder.ScreenwriterSeedingArray)
            {
                await applicationDbContext.Screenwriters.AddAsync(screenwriterToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var filmProductionToSeed in FilmProductionSeeder.FilmProductionSeedingArray)
            {
                await applicationDbContext.FilmProductions.AddAsync(filmProductionToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var filmProductionActorsToSeed
                in FilmProductionActorSeeder.FilmProductionActorSeedingArray
            )
            {
                await applicationDbContext.FilmProductionActors
                    .AddAsync(filmProductionActorsToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var filmProductionDirectorsToSeed
                in FilmProductionDirectorSeeder.FilmProductionDirectorSeedingArray
            )
            {
                await applicationDbContext.FilmProductionDirectors
                    .AddAsync(filmProductionDirectorsToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var filmProductionGenresToSeed
                in FilmProductionGenreSeeder.FilmProductionGenreSeedingArray
            )
            {
                await applicationDbContext.FilmProductionGenres
                    .AddAsync(filmProductionGenresToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var filmProductionScreenwritersToSeed
                in FilmProductionScreenwriterSeeder.FilmProductionScreenwriterSeedingArray
            )
            {
                await applicationDbContext.FilmProductionScreenwriters
                    .AddAsync(filmProductionScreenwritersToSeed);
            }

            await applicationDbContext.SaveChangesAsync();

            foreach (var subtitlesToSeed in SubtitlesSeeder.SubtitlesSeedingArray)
            {
                await applicationDbContext.Subtitles.AddAsync(subtitlesToSeed);
            }

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
