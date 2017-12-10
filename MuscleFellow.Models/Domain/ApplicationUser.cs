using Microsoft.AspNetCore.Identity;
using System;

namespace MuscleFellow.Models.Domain
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString("D");
        }
    }
}