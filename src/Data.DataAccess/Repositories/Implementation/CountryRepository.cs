using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.DataAccess.Repositories.Implementation
{
    public class CountryRepository: BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<Country> countries, Country countryToFind)
        {
            Expression<Func<Country, bool>> countryExistsPredicate = c => 
                c.Name.Trim().ToLower() == countryToFind.Name;

            bool countryExists = countries.Any(countryExistsPredicate);

            return countryExists;
        }
    }
}
