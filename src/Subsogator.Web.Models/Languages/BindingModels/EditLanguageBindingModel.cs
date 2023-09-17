using Subsogator.Common.GlobalConstants;
using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Models.Languages.BindingModels
{
    public class EditLanguageBindingModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5,
            ErrorMessage = ValidationConstants.LanguageNameMinimumLengthValidationMessage)]
        public string Name { get; set; }
    }
}
