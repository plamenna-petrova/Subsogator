using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.DataAccess.Seeding
{
    public class EntitiesSeeder : ISeeder
    {
        public bool SeedDatabase(ApplicationDbContext applicationDbContext)
        {
            if (applicationDbContext.Subtitles.Any())
            {
                return false;
            }

            ExecutePartialSeeders(applicationDbContext);

            return false;
        }

        private void ExecutePartialSeeders(ApplicationDbContext applicationDbContext)
        {
            foreach (var actorToSeed in ActorSeeder.ActorSeedingArray)
            {
                applicationDbContext.Actors.Add(actorToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var countryToSeed in CountrySeeder.CountrySeedingArray)
            {
                applicationDbContext.Countries.Add(countryToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var directorToSeed in DirectorSeeder.DirectorSeedingArray)
            {
                applicationDbContext.Directors.Add(directorToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var genreToSeed in GenreSeeder.GenreSeedingArray)
            {
                applicationDbContext.Genres.Add(genreToSeed);
            }

            applicationDbContext.SaveChanges();


            foreach (var languageToSeed in LanguageSeeder.LanguageSeedingArray)
            {
                applicationDbContext.Languages.Add(languageToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var screenwriterToSeed in ScreenwriterSeeder.ScreenwriterSeedingArray)
            {
                applicationDbContext.Screenwriters.Add(screenwriterToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var filmProductionToSeed in FilmProductionSeeder.FilmProductionSeedingArray)
            {
                applicationDbContext.FilmProductions.Add(filmProductionToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var filmProductionActorsToSeed
                in FilmProductionActorSeeder.FilmProductionActorSeedingArray
            )
            {
                applicationDbContext.FilmProductionActors.Add(filmProductionActorsToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var filmProductionDirectorsToSeed
                in FilmProductionDirectorSeeder.FilmProductionDirectorSeedingArray
            )
            {
                applicationDbContext.FilmProductionDirectors.Add(filmProductionDirectorsToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var filmProductionGenresToSeed
                in FilmProductionGenreSeeder.FilmProductionGenreSeedingArray
            )
            {
                applicationDbContext.FilmProductionGenres.Add(filmProductionGenresToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var filmProductionScreenwritersToSeed
                in FilmProductionScreenwriterSeeder.FilmProductionScreenwriterSeedingArray
            )
            {
                applicationDbContext.FilmProductionScreenwriters.Add(filmProductionScreenwritersToSeed);
            }

            applicationDbContext.SaveChanges();

            foreach (var subtitlesToSeed in SubtitlesSeeder.SubtitlesSeedingArray)
            {
                applicationDbContext.Subtitles.Add(subtitlesToSeed);
            }

            applicationDbContext.SaveChanges();
        }
    }
}
