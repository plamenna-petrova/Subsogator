using Microsoft.AspNetCore.Identity;

namespace Data.DataModels.Entities.Identity
{
    public class ApplicationUserRole: IdentityUserRole<string>
    {
        public virtual ApplicationRole Role { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
