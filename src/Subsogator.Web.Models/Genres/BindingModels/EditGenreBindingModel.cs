using Subsogator.Common.GlobalConstants;
using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Models.Genres.BindingModels
{
    public class EditGenreBindingModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5,
            ErrorMessage = ValidationConstants.CountryNameMinimumLengthValidationMessage)]
        public string Name { get; set; }
    }
}
