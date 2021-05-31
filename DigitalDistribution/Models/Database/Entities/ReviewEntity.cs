using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Reviews")]
    public class ReviewEntity:BaseEntity
    {
        public int ProductId { get; set; }
        public int ProfileId { get; set; }
        public float Rating { get; set; }
        public float Content { get; set; }
       
        [ForeignKey("ProductId")] public ProductEntity Product { get; set; }
        [ForeignKey("ProfileId")]public ProfileEntity Profile { get; set; }
    }
}
