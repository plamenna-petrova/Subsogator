using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.BindingModels
{
    public class EditFilmProductionBindingModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2,
            ErrorMessage = ValidationConstants.FilmProductionTitleMinimumLengthValidationMessage)]
        public string Title { get; set; }

        [Required]
        [Range(45, 240, ErrorMessage = ValidationConstants.FilmProductionDurationRangeValidationMessage)]
        public int Duration { get; set; }

        [Required]
        [DisplayName(DisplayConstants.FilmProductionReleaseDateDisplayName)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 2,
                ErrorMessage = ValidationConstants.FilmProductionPlotSummaryMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.FilmProductionPlotSummaryDisplayName)]
        public string PlotSummary { get; set; }

        public string CountryId { get; set; }

        public string LanguageId { get; set; }
    }
}
