using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class UserRoleEntity: IdentityUserRole<int>
    {
        [ForeignKey("UserId")] public UserEntity User { get; set; }
        [ForeignKey("RoleId")] public RoleEntity Role { get; set; }
    }
}
