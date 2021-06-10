using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Products")]
    public class ProductEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Rating { get; set; } = 0;
        public float Price { get; set; }
        public float Version { get; set; } = 0.1f;
        public string Currency { get; set; } = "USD";
        public int DevTeamId { get; set; }
        [ForeignKey("DevTeamId")] public DevelopmentTeamEntity DevTeam { get; set; }
        public List<UpdateEntity> Updates { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
        public List<LibraryProductEntity> LibraryItems { get; set; }
        public List<CheckoutItemEntity> InvoiceItems { get; set; }

    }
}
