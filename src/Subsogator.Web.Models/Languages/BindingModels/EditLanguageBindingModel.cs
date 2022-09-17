using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
