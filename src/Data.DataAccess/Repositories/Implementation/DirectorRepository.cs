using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class DirectorRepository: CrewMemberRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
