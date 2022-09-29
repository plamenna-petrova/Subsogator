using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class FilmProductionDirectorRepository: BaseRepository<FilmProductionDirector>, 
        IFilmProductionDirectorRepository
    {
        public FilmProductionDirectorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
