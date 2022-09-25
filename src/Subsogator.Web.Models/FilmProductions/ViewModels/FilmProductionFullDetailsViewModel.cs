using Subsogator.Web.Models.Actors.ViewModels;
using Subsogator.Web.Models.Directors.ViewModels;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class FilmProductionFullDetailsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Plot Summary")]
        public string PlotSummary { get; set; }

        [DisplayName("Country")]
        public string CountryName { get; set; }

        [DisplayName("Language")]
        public string LanguageName { get; set; }

        [DisplayName("Genres")]
        public List<string> RelatedGenres { get; set; }

        [DisplayName("Actors")]
        public List<Tuple<string, string>> RelatedActors { get; set; }

        [DisplayName("Directors")]
        public List<Tuple<string, string>> RelatedDirectors { get; set; }

        [DisplayName("Screenwriters")]
        public List<Tuple<string, string>> RelatedScreenwriters { get; set; }
    }
}
