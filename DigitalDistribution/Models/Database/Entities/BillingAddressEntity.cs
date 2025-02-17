﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalDistribution.Models.Database.Entities
{
    [Table("Addresses")]
    public class BillingAddressEntity:BaseEntity
    {
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public List<InvoiceEntity> Bills { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]public UserEntity User { get; set; }
    }
}
