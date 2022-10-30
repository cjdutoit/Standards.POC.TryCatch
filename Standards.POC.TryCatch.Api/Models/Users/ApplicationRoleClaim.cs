using System;
using Microsoft.AspNetCore.Identity;

namespace Standards.POC.TryCatch.Api.Models.Users
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
