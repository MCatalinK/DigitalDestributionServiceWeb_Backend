using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("updates")]
    public class UpdateEntity
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }
    }

}
