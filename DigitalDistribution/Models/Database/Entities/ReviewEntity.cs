using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Reviews")]
    public class ReviewEntity:BaseEntity
    {
        public int Rating { get; set; }
        public string Content { get; set; }
        public int ProductId { get; set; }
        public int ProfileId { get; set; }

        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }
        [ForeignKey("ProfileId")]public ProfileEntity Profile { get; set; }
    }
}
