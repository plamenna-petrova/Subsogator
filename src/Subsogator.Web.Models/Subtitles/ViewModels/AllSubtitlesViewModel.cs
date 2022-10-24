using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Subtitles.ViewModels
{
    public class AllSubtitlesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public FilmProductionConciseInformationViewModel RelatedFilmProduction { get; set; }
    }
}
