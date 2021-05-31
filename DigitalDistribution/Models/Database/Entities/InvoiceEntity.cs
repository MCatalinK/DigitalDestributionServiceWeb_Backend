using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Invoices")]
    public class InvoiceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]public UserEntity User { get; set; }
        public float Price { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CheckoutItemEntity> CheckoutItems { get; set; }
        public bool IsPayed { get; set; }       
        
    }
}
