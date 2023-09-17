using Data.DataModels.Entities;
using System;

namespace Data.DataAccess.Seeding
{
    internal static class LanguageSeeder
    {
        internal static Language[] LanguageSeedingArray { get; private set; } = SeedLanguages();

        private static Language[] SeedLanguages() 
        {
            var languagesToSeed = new Language[]
            {
                new Language() 
                {
                    Name = "English",
                    CreatedOn = DateTime.UtcNow
                },
                new Language() 
                {
                    Name = "German",
                    CreatedOn = DateTime.UtcNow
                },
                new Language() 
                {
                    Name = "Spanish",
                    CreatedOn = DateTime.UtcNow
                },
                new Language() 
                {
                    Name = "Italian",
                    CreatedOn = DateTime.UtcNow
                },
                new Language() 
                {
                    Name = "Russian",
                    CreatedOn = DateTime.UtcNow
                }
            };

            return languagesToSeed;
        }
    }
}
