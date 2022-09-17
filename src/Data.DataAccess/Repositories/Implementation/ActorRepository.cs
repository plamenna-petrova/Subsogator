using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Subsogator.Infrastructure.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class ActorRepository: BaseRepository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public override bool Exists(IQueryable<Actor> actors, Actor actorToFind)
        {
            Expression<Func<Actor, bool>> actorExistsPredicate = a =>
                    a.FirstName.Trim().ToLower() ==
                    actorToFind.FirstName.Trim().ToLower() && 
                    a.LastName.Trim().ToLower() ==
                    actorToFind.LastName.Trim().ToLower();

            bool actorExists = actors.Any(actorExistsPredicate);

            return actorExists;
        }
    }
}
