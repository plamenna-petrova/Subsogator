using Subsogator.Web.Models.Comments.ViewModels;
using Subsogator.Web.Models.Favourites.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.SubtitlesCatalogue
{
    public class CatalogueItemsViewModel
    {
        public IEnumerable<AllSubtitlesForCatalogueViewModel> AllSubtitlesForCatalogue { get; set; }

        public IEnumerable<LatestCommentViewModel> LatestComments { get; set; }

        public IEnumerable<TopSubtitlesViewModel> TopSubtitles { get; set; }
    }
}
