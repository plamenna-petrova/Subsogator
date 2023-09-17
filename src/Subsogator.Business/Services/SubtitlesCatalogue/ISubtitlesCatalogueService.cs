using Subsogator.Web.Models.SubtitlesCatalogue;
using System.Collections.Generic;

namespace Subsogator.Business.Services.SubtitlesCatalogue
{
    public interface ISubtitlesCatalogueService
    {
        IEnumerable<AllSubtitlesForCatalogueViewModel> GetAllSubtitlesForCatalogue();

        SubtitlesCatalogueItemDetailsViewModel GetSubtitlesCatalogueItemDetails(string subtitlesId);
    }
}
