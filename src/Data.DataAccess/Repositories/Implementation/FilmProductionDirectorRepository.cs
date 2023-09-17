using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

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
