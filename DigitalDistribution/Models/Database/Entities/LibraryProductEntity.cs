using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class LibraryProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Licence { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
       
        [ForeignKey("UserId")]public UserEntity User { get; set; }
        [ForeignKey("ProductId")]public ProductEntity Product { get; set; }
    }
}
