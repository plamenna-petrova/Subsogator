using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class UserRepository: BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
