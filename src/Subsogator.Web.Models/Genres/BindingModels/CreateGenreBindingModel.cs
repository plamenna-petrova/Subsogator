using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Genres.BindingModels
{
    public class CreateGenreBindingModel
    {
        [Required]
        [StringLength(15, MinimumLength = 5,
            ErrorMessage = ValidationConstants.GenreNameMinimumLengthValidationMessage)]
        public string Name { get; set; }
    }
}
