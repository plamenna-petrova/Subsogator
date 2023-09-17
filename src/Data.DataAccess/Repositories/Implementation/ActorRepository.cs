using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

namespace Data.DataAccess.Repositories.Implementation
{
    public class ActorRepository: CrewMemberRepository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
