using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class ScreenwriterRepository: CrewMemberRepository<Screenwriter>, 
        IScreenwriterRepository
    {
        public ScreenwriterRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
