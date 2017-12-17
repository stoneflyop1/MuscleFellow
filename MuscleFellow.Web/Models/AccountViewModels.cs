using System;
using System.Collections.Generic;
using MuscleFellow.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace MuscleFellow.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
