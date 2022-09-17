using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class DirectorRepository: BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<Director> directors, Director directorToFind)
        {
            Expression<Func<Director, bool>> directorExistsPredicate = d =>
                    d.FirstName.Trim().ToLower() ==
                    directorToFind.FirstName.Trim().ToLower() &&
                    d.LastName.Trim().ToLower() ==
                    directorToFind.LastName.Trim().ToLower();

            bool directorExists = directors.Any(directorExistsPredicate);

            return directorExists;
        }
    }
}
