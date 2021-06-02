using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table ("Profiles")]
    public class ProfileEntity:BaseEntity
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; } = "DefaultPicture";
        public string Description { get; set; } = "";
        public DateTime LastOnline { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        [ForeignKey("UserId")] public UserEntity User { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
    }
}
