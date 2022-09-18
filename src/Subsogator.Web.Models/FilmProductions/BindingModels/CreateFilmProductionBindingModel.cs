using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.BindingModels
{
    public class CreateFilmProductionBindingModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2,
            ErrorMessage = ValidationConstants.FilmProductionTitleMinimumLengthValidationMessage)]
        public string Title { get; set; }

        [Required]
        [Range(45, 240, ErrorMessage = ValidationConstants.FilmProductionDurationRangeValidationMessage)]
        public int Duration { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 2, 
                ErrorMessage = ValidationConstants.FilmProductionPlotSummaryMinimumLengthValidationMessage)]
        public string PlotSummary { get; set; }

        public string CountryId { get; set; }

        public string LanguageId { get; set; }
    }
}
