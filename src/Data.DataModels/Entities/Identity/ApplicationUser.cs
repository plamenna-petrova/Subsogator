using Data.DataModels.Enums;
using Data.DataModels.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.DataModels.Entities.Identity
{
    public class ApplicationUser: IdentityUser, IAuditInfo
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 7);
            ApplicationUserRoles = new HashSet<ApplicationUserRole>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
            Subtitles = new HashSet<Subtitles>();
            Comments = new HashSet<Comment>();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public PromotionStatus PromotionStatus { get; set; } = PromotionStatus.Neutral;

        public string PromotionLevel { get; set; }

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Subtitles> Subtitles { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
