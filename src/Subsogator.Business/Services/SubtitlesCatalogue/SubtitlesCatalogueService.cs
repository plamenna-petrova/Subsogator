using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Enums;
using Subsogator.Web.Models.Subtitles.ViewModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.SubtitlesCatalogue
{
    public class SubtitlesCatalogueService: ISubtitlesCatalogueService
    {
        private ISubtitlesRepository _subtitlesRepository;

        private IUserRepository _userRepository;

        public SubtitlesCatalogueService(
            ISubtitlesRepository subtitlesRepository,
            IUserRepository userRepository
        )
        {
            _subtitlesRepository = subtitlesRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<AllSubtitlesForCatalogueViewModel> GetAllSubtitlesForCatalogue()
        {
            List<AllSubtitlesForCatalogueViewModel> allSubtitlesForCatalogue = _subtitlesRepository
                .GetAllAsNoTracking()
                    .Select(s => new AllSubtitlesForCatalogueViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        UploaderUserName = s.ApplicationUser.UserName,
                        RelatedFilmProduction = new FilmProductionForCatalogueViewModel
                        {
                            Title = s.FilmProduction.Title,
                            Duration = s.FilmProduction.Duration,
                            ReleaseDate = s.FilmProduction.ReleaseDate,
                            PlotSummary = s.FilmProduction.PlotSummary,
                            CountryName = s.FilmProduction.Country.Name,
                            LanguageName = s.FilmProduction.Language.Name,
                            ImageName = s.FilmProduction.ImageName
                        }
                    })
                    .ToList();

            return allSubtitlesForCatalogue;
        }

        public SubtitlesCatalogueItemDetailsViewModel GetSubtitlesCatalogueItemDetails(string subtitlesId)
        {
            var singleSubtitlesCatalogueItem = _subtitlesRepository
               .GetAllByCondition(s => s.Id == subtitlesId)
                   .FirstOrDefault();

            if (singleSubtitlesCatalogueItem is null)
            {
                return null;
            }

            var singleSubtitlesDetails = new SubtitlesCatalogueItemDetailsViewModel
            {
                Id = singleSubtitlesCatalogueItem.Id,
                Name = singleSubtitlesCatalogueItem.Name,
                CreatedOn = singleSubtitlesCatalogueItem.CreatedOn,
                ModifiedOn = singleSubtitlesCatalogueItem.ModifiedOn,
                UploaderUserName = singleSubtitlesCatalogueItem.ApplicationUser.UserName,
                RelatedFilmProduction = new FilmProductionForCatalogueViewModel
                {
                    Title = singleSubtitlesCatalogueItem.FilmProduction.Title,
                    Duration = singleSubtitlesCatalogueItem.FilmProduction.Duration,
                    ReleaseDate = singleSubtitlesCatalogueItem.FilmProduction.ReleaseDate,
                    PlotSummary = singleSubtitlesCatalogueItem.FilmProduction.PlotSummary,
                    CountryName = singleSubtitlesCatalogueItem.FilmProduction.Country.Name,
                    LanguageName = singleSubtitlesCatalogueItem.FilmProduction.Language.Name,
                    ImageName = singleSubtitlesCatalogueItem.FilmProduction.ImageName
                }
            };

            return singleSubtitlesDetails;
        }
    }
}
