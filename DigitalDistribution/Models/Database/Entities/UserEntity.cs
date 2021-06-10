using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public bool IsActive { get; set; }
        public bool TermsAccepted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
        public ProfileEntity Profile { get; set; }
        public BillingAddressEntity Address { get; set; }
        public List<UserRoleEntity> UserRoles { get; set; }
        public List<LibraryProductEntity> LibraryItems { get; set; } 
        public int? DevTeamId { get; set; }
        [ForeignKey("DevTeamId")] public DevelopmentTeamEntity DevTeam { get; set; }
    }
}
