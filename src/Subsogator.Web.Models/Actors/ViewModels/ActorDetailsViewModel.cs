using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Actors.ViewModels
{
    public class ActorDetailsViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Modified On")]
        [DisplayFormat(NullDisplayText = "Not Yet Modified")]
        public DateTime? ModifiedOn { get; set; }
    }
}
