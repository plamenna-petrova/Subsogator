using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

namespace Data.DataAccess.Repositories.Implementation
{
    public class FilmProductionGenreRepository: BaseRepository<FilmProductionGenre>, 
        IFilmProductionGenreRepository
    {
        public FilmProductionGenreRepository(ApplicationDbContext applicationDbContext)
            : base (applicationDbContext)
        {

        }
    }
}
