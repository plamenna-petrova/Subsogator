using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class FilmProductionActorRepository: BaseRepository<FilmProductionActor>, IFilmProductionActorRepository
    {
        public FilmProductionActorRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}
