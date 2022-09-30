using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Mapping
{
    public class AssignedScreenwriterDataViewModel
    {
        public string ScreenwriterId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
