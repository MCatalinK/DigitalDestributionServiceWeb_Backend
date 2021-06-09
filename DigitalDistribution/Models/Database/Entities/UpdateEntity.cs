using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Updates")]
    public class UpdateEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Version { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UpdateEntity entity &&
                   Version == entity.Version;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Version);
        }
    }
}
