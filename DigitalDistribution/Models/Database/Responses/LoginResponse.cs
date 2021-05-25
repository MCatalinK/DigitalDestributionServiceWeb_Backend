using DigitalDistribution.Models.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace DigitalDistribution.Models.Database.Responses
{
    public class LoginResponse
    {
        public SignInResult Result { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserEntity User { get; set; }

    }
}
