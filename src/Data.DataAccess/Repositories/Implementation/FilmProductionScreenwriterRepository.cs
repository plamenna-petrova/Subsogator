using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

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
