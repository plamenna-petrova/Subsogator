using Data.DataModels.Entities;
using System;

namespace Data.DataAccess.Seeding
{
    internal static class CountrySeeder
    {
        internal static Country[] CountrySeedingArray { get; private set; } = SeedCountries();

        private static Country[] SeedCountries() 
        {
            var countriesToSeed = new Country[]
            {
                new Country()
                {
                    Name = "USA",
                    CreatedOn = DateTime.UtcNow
                },
                new Country() 
                {
                    Name = "UK",
                    CreatedOn = DateTime.UtcNow
                },
                new Country() 
                {
                    Name = "Germany",
                    CreatedOn = DateTime.UtcNow
                },
                new Country() 
                {
                    Name = "Spain",
                    CreatedOn = DateTime.UtcNow
                },
                new Country() 
                {
                    Name = "Australia",
                    CreatedOn = DateTime.UtcNow
                }
            };

            return countriesToSeed;
        }
    }
}
