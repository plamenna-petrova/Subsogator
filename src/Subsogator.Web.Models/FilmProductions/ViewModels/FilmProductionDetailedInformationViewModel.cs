using System;
using System.ComponentModel;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class FilmProductionDetailedInformationViewModel
    {
        public string Title { get; set; }

        public int Duration { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
    }
}
