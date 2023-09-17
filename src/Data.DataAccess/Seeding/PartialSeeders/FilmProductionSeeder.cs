using Data.DataModels.Entities;
using System;
using System.Linq;

namespace Data.DataAccess.Seeding
{
    internal static class FilmProductionSeeder
    {
        internal static FilmProduction[] FilmProductionSeedingArray { get; private set; } = SeedFilmProductions();

        private static FilmProduction[] SeedFilmProductions()
        {
            Func<Country, bool> ukCountryPredicate = c => c.Name == "UK";
            Func<Country, bool> usaCountryPredicate = c => c.Name == "USA";
            Func<Language, bool> englishLanguagePredicate = l => l.Name == "English";

            var filmProductionsToSeed = new FilmProduction[]
            {
                new FilmProduction()
                {
                    Title = "Inception",
                    Duration = 148,
                    ReleaseDate = DateTime.Parse("2010-07-23"),
                    PlotSummary = "A thief who steals corporate secrets through " +
                    "the use of dream-sharing technology is given the inverse task of " +
                    "planting an idea into the mind of a C.E.O., " +
                    "but his tragic past may doom the project and his team to disaster.",
                    CountryId = CountrySeeder.CountrySeedingArray
                        .Single(c => c.Name == "USA").Id,
                    LanguageId = LanguageSeeder.LanguageSeedingArray
                        .Single(englishLanguagePredicate).Id,
                },
                new FilmProduction()
                {
                    Title = "The Bourne Supremacy",
                    Duration = 108,
                    ReleaseDate = DateTime.Parse("2004-12-03"),
                    PlotSummary = "When Jason Bourne is framed for a CIA operation gone awry, " +
                    "he is forced to resume his former life as a trained assassin to survive.",
                    CountryId = CountrySeeder.CountrySeedingArray
                        .Single(c => c.Name == "Germany").Id,
                    LanguageId = LanguageSeeder.LanguageSeedingArray
                        .Single(englishLanguagePredicate).Id
                },
                new FilmProduction()
                {
                    Title = "V for Vendetta",
                    Duration = 132,
                    ReleaseDate = DateTime.Parse("2006-04-21"),
                    PlotSummary = "In a future British dystopian society, a shadowy freedom fighter, " +
                    "known only by the alias of \"V\"," +
                    " plots to overthrow the tyrannical government - with the help of a young woman.",
                    CountryId = CountrySeeder.CountrySeedingArray
                        .Single(ukCountryPredicate).Id,
                    LanguageId = LanguageSeeder.LanguageSeedingArray
                        .Single(englishLanguagePredicate).Id
                },
                new FilmProduction()
                {
                    Title = "Batman Begins",
                    Duration = 140,
                    ReleaseDate = DateTime.Parse("2005-06-17"),
                    PlotSummary = "After training with his mentor, " +
                    "Batman begins his fight to free crime-ridden Gotham City from corruption.",
                    CountryId = CountrySeeder.CountrySeedingArray
                        .Single(ukCountryPredicate).Id,
                    LanguageId = LanguageSeeder.LanguageSeedingArray
                        .Single(englishLanguagePredicate).Id
                },
                new FilmProduction()
                {
                    Title = "Interstellar",
                    Duration = 169,
                    ReleaseDate = DateTime.Parse("2014-11-07"),
                    PlotSummary = "A team of explorers travel through a wormhole in space " +
                    "in an attempt to ensure humanity's survival.",
                    CountryId = CountrySeeder.CountrySeedingArray
                        .Single(usaCountryPredicate).Id,
                    LanguageId = LanguageSeeder.LanguageSeedingArray
                        .Single(englishLanguagePredicate).Id
                }
            };

            return filmProductionsToSeed;
        }
    }
}
