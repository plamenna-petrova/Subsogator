using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Web.Models.Languages.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class AllFilmProductionsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Language")]
        public LanguageConciseInformationViewModel RelatedLanguage { get; set; }

        [DisplayName("Country")]
        public CountryConciseInformationViewModel RelatedCountry { get; set; }
    }
}
