using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities.Identity;

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
