using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Products")]
    public class ProductEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public float Price { get; set; }
        public int DeveloperId { get; set; }
        [ForeignKey("DeveloperId")] public DevelopmentTeamEntity Developer { get; set; }
        public List<UpdateEntity> Updates { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
        public List<LibraryProductEntity> LibraryItems { get; set; }
        public List<CheckoutItemEntity> InvoiceItems { get; set; }
    }
}
