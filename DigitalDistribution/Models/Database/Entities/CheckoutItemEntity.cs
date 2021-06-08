using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class CheckoutItemEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public string Licence { get; set; }
        [ForeignKey("ProductId")]public ProductEntity Product { get; set; }
        [ForeignKey("InvoiceId")]public InvoiceEntity Invoice { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CheckoutItemEntity entity &&
                   ProductId == entity.ProductId &&
                   InvoiceId == entity.InvoiceId;
        }
    }
}
