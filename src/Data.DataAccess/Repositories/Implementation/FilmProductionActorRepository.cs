using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

namespace Data.DataAccess.Repositories.Implementation
{
    public class FilmProductionActorRepository: BaseRepository<FilmProductionActor>, IFilmProductionActorRepository
    {
        public FilmProductionActorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
