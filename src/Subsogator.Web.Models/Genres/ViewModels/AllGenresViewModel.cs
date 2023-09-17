using Subsogator.Web.Models.FilmProductions.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Web.Models.Genres.ViewModels
{
    public class AllGenresViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<FilmProductionConciseInformationViewModel> RelatedFilmProductions { get; set; }
    }
}
