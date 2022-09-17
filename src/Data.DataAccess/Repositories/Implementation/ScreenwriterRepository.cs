using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class ScreenwriterRepository: BaseRepository<Screenwriter>, IScreenwriterRepository
    {
        public ScreenwriterRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<Screenwriter> screenwriters, Screenwriter screenwriterToFind)
        {
            Expression<Func<Screenwriter, bool>> screenwriterExistsPredicate = s =>
                    s.FirstName.Trim().ToLower() ==
                    screenwriterToFind.FirstName.Trim().ToLower() &&
                    s.LastName.Trim().ToLower() ==
                    screenwriterToFind.LastName.Trim().ToLower();

            bool screenwriterExists = screenwriters.Any(screenwriterExistsPredicate);

            return screenwriterExists;
        }
    }
}
