using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuscleFellow.Web.Jwt
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/api/v1/Account/Login";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(500);
        public SigningCredentials SigningCredentials { get; set; }
    }
}
