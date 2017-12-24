using Microsoft.AspNetCore.Identity;
using System;

namespace MuscleFellow.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString("D");
        }
    }
}