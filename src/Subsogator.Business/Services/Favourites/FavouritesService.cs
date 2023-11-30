using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Subsogator.Web.Models.Favourites.ViewModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Subtitles.ViewModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Subsogator.Business.Services.Favourites
{
    public class FavouritesService : IFavouritesService
    {
        private readonly IFavouritesRepository _favouritesRepository;

        private readonly ISubtitlesRepository _subtitlesRepository;

        public FavouritesService(
            IFavouritesRepository favouritesRepository, 
            ISubtitlesRepository subtitlesRepository
        )
        {
            _favouritesRepository = favouritesRepository;
            _subtitlesRepository = subtitlesRepository;
        }

        public IEnumerable<UserFavouritesViewModel> GetAllUserFavourites(string userId)
        {
            List<string> allUserFavouritesSubtitlesIds = _favouritesRepository
                .GetAllByCondition(f => f.ApplicationUserId == userId)
                    .Select(f => f.SubtitlesId)
                        .ToList();

            List < UserFavouritesViewModel > allUserFavourites = _subtitlesRepository
                .GetAllByCondition(s => allUserFavouritesSubtitlesIds.Contains(s.Id))
                    .Select(s => new UserFavouritesViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        UploaderUserName = s.ApplicationUser.UserName,
                        RelatedFilmProduction = new FilmProductionConciseInformationViewModel
                        {
                            Title = s.FilmProduction.Title
                        }
                    })
                    .OrderBy(uf => uf.Name)
                    .ToList();

            return allUserFavourites;
        }

        public bool AddToFavourites(string userId, string subtitlesId)
        {
            Data.DataModels.Entities.Favourites favouritesToCreate = new Data.DataModels.Entities.Favourites
            {
                ApplicationUserId = userId,
                SubtitlesId = subtitlesId
            };

            var allFavourites = _favouritesRepository.GetAllAsNoTracking();

            if (_favouritesRepository.Exists(allFavourites, favouritesToCreate))
            {
                return false;
            }

            _favouritesRepository.Add(favouritesToCreate);

            return true;
        }

        public void RemoveFromFavourites(Data.DataModels.Entities.Favourites favourites) 
        {
            _favouritesRepository.Delete(favourites);
        }

        public Data.DataModels.Entities.Favourites FindFavourites(string userId, string subtitlesId)
        {
            return _favouritesRepository
                .GetAllByCondition(f => f.ApplicationUserId == userId && f.SubtitlesId == subtitlesId)
                    .FirstOrDefault();
        }
    }
}
