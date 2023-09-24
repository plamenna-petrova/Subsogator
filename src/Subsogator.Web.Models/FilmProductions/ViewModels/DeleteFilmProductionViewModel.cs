using System;
using System.ComponentModel;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class DeleteFilmProductionViewModel
    {
        public string Title { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Image")]
        public string ImageName { get; set; }
    }
}
