using System;

namespace MuscleFellow.ApiModels
{
    public class LoginAPIModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }

    public class TokenEntity
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}