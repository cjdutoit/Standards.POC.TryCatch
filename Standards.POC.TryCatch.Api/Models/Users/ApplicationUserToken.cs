using System;
using Microsoft.AspNetCore.Identity;

namespace Standards.POC.TryCatch.Api.Models.Users
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}