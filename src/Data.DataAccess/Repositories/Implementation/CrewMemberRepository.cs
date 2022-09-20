using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public abstract class CrewMemberRepository<TCrewMemberEntity> : BaseRepository<TCrewMemberEntity>, 
            ICrewMemberRepository<TCrewMemberEntity> where TCrewMemberEntity: CrewMember
    {
        public CrewMemberRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<TCrewMemberEntity> crewMemberEntities, 
            TCrewMemberEntity crewMemberEntityToFind)
        {
            Expression<Func<TCrewMemberEntity, bool>> existingCrewMemberPredicate = cm =>
                cm.FirstName.Trim().ToLower() ==
                    crewMemberEntityToFind.FirstName.Trim().ToLower() &&
                cm.LastName.Trim().ToLower() ==
                    crewMemberEntityToFind.LastName.Trim().ToLower();

            bool crewMemberExists = crewMemberEntities.Any(existingCrewMemberPredicate);

            return crewMemberExists;
        }
    }
}
