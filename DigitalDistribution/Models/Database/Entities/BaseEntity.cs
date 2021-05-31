using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateModified { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
