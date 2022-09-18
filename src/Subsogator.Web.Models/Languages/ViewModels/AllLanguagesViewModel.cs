using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Languages.ViewModels
{
    public class AllLanguagesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<FilmProductionConciseInformationViewModel> RelatedFilmProductions { get; set; }
    }
}
