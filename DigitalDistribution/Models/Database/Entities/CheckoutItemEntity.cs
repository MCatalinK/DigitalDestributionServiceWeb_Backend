using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class CheckoutItemEntity
    {
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public string License { get; set; }
        [ForeignKey("ProductId")]public ProductEntity Product { get; set; }
        [ForeignKey("InvoiceId")]public InvoiceEntity Invoice { get; set; }  
    }
}
