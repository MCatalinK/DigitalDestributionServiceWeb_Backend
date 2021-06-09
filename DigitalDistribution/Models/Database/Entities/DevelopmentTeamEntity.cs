using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("DevelopmentTeams")]
    public class DevelopmentTeamEntity:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductEntity> Products { get; set; }
        public List<UserEntity> Users { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DevelopmentTeamEntity entity &&
                   Name == entity.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
