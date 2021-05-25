using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("reviews")]
    public class ReviewEntity:BaseEntity
    {
        public float Rating { get; set; }
        public float Content { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }
        public int ProfileId { get; set; }
        [ForeignKey("ProfileId")]public ProfileEntity Profile { get; set; }
    }
}
