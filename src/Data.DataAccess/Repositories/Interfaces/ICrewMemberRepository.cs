using Data.DataModels.Entities;

namespace Data.DataAccess.Repositories.Interfaces
{
    public interface ICrewMemberRepository<TCrewMemberEntity> 
        : IBaseRepository<TCrewMemberEntity> 
        where TCrewMemberEntity : CrewMember
    {

    }
}
