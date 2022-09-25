using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class DeleteFilmProductionViewModel
    {
        public string Title { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
    }
}
