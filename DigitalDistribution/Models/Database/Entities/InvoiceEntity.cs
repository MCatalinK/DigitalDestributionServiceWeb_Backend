using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Invoices")]
    public class InvoiceEntity:BaseEntity
    {
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]public BillingAddressEntity Address { get; set; }
        public float Price { get; set; } = 0;
        public List<CheckoutItemEntity> CheckoutItems { get; set; }
        public bool IsPayed { get; set; } = false; 
        
    }
}
