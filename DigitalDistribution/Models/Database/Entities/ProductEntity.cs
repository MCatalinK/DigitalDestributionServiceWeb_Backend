using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class ProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public float Price { get; set; }
        public int IdDeveloper { get; set; }
        [ForeignKey("IdDeveloper")] public DeveloperEntity Developer { get; set; }
        public List<UpdateEntity> Updates { get; set; }
        public List<ReviewEntity> Reviews { get; set; }
        public List<LibraryProductEntity> LibraryItems { get; set; }
        public List<WishlistProductEntity> WishlistsItems { get; set; }
        public List<CheckoutItemEntity> InvoiceItems { get; set; }
    }
}
