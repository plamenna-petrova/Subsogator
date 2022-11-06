using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Users.ViewModels
{
    public class AllUsersViewModel
    {
        public string Username { get; set; }

        public List<string> Roles { get; set; }
    }
}
