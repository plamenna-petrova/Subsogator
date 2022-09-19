using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class FilmProductionFullDetailsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string PlotSummary { get; set; }

        public string CountryName { get; set; }

        public string LanguageName { get; set; }
    }
}
