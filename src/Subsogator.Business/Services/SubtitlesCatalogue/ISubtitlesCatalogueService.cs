using Subsogator.Web.Models.Subtitles.ViewModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.SubtitlesCatalogue
{
    public interface ISubtitlesCatalogueService
    {
        IEnumerable<AllSubtitlesForCatalogueViewModel> GetAllSubtitlesForCatalogue();

        SubtitlesCatalogueItemDetailsViewModel GetSubtitlesCatalogueItemDetails(string subtitlesId);

        void EnrollForUploaderRole(string userId);
    }
}
