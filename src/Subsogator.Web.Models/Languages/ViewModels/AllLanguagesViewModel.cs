using Subsogator.Web.Models.FilmProductions.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Web.Models.Languages.ViewModels
{
    public class AllLanguagesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<FilmProductionConciseInformationViewModel> RelatedFilmProductions { get; set; }
    }
}
