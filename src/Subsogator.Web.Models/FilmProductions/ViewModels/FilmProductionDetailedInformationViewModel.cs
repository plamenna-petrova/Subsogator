using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
