using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public class FilmProductionScreenwriterRepository: BaseRepository<FilmProductionScreenwriter>,
        IFilmProductionScreenwriterRepository
    {
        public FilmProductionScreenwriterRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
