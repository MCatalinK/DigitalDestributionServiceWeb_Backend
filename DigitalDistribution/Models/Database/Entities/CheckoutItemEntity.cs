using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class CheckoutItemEntity
    {
        public string LicenseKey { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]public ProductEntity Product { get; set; }
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]public InvoiceEntity Invoice { get; set; }
    }
}
