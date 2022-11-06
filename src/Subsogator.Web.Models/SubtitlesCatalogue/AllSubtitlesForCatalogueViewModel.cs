using Data.DataModels.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.SubtitlesCatalogue
{
    public class AllSubtitlesForCatalogueViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UploaderUserName { get; set; }

        public FilmProductionForCatalogueViewModel RelatedFilmProduction { get; set; }
    }
}
