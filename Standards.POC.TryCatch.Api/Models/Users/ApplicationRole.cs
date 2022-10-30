using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Standards.POC.TryCatch.Api.Models.Users
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
