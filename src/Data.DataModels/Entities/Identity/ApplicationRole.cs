using Data.DataModels.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataModels.Entities.Identity
{
    public class ApplicationRole: IdentityRole, IAuditInfo
    {
        public ApplicationRole()
            : this(null)
        {
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
        }

        public ApplicationRole(string name)
            : base(name)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 7);
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }
    }
}
