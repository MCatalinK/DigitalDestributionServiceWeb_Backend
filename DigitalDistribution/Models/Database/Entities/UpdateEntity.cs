using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Updates")]
    public class UpdateEntity
    {
        public int ProductId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }
    }

}
