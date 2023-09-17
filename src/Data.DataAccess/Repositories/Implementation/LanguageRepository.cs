using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.DataAccess.Repositories.Implementation
{
    public class LanguageRepository: BaseRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<Language> languages, Language languageToFind)
        {
            Expression<Func<Language, bool>> languageExistsPredicate = l =>
                l.Name.Trim().ToLower() == languageToFind.Name;

            bool languageExists = languages.Any(languageExistsPredicate);

            return languageExists;
        }
    }
}
