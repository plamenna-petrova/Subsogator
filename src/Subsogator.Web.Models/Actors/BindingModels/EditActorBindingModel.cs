using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Actors.BindingModels
{
    public class EditActorBindingModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "The first name of the " +
    "actor cannot be shorter than 2 symbols")]
        [MaxLength(25, ErrorMessage = "The first name " +
    "of the actor cannot be longer than 25 symbols")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "The last name of the" +
            " actor cannot be shorter than 5 symbols")]
        [MaxLength(25, ErrorMessage = "The last name of " +
            "the actor cannot be longer than 25 symbols")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}
