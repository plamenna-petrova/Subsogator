using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.DataAccess.Repositories.Interfaces
{
    public interface ICrewMemberRepository<TCrewMemberEntity> 
        : IBaseRepository<TCrewMemberEntity> 
        where TCrewMemberEntity : CrewMember
    {

    }
}
