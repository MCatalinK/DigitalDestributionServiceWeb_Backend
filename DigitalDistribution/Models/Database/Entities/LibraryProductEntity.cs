using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class LibraryProductEntity
    {
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]public UserEntity User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]public ProductEntity Product { get; set; }
    }
}
