using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;

namespace Data.DataAccess.Repositories.Implementation
{
    public class SubtitlesRepository: BaseRepository<Subtitles>, ISubtitlesRepository
    {
        public SubtitlesRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
