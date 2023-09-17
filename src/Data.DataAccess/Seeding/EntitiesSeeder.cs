using Data.DataModels.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Subsogator.Common.GlobalConstants;
using System;
using System.Linq;
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

            await ExecutePartialSeeders(applicationDbContext, serviceProvider);

            return true;
        }

        private async Task ExecutePartialSeeders(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
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
                await applicationDbContext.Directors.AddAsync(directorToSeed);
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

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var databaseLoadedUsers = applicationDbContext.Users.IgnoreQueryFilters().ToList();

            var administrator = databaseLoadedUsers
                .Where(dlu => dlu.ApplicationUserRoles
                    .Select(aur => aur.Role.Name).First() == IdentityConstants.AdministratorRoleName
                )
                .FirstOrDefault();

            foreach (var subtitlesToSeed in SubtitlesSeeder.SubtitlesSeedingArray)
            {
                subtitlesToSeed.ApplicationUser = administrator;
                await applicationDbContext.Subtitles.AddAsync(subtitlesToSeed);
            }

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
