using DigitalDistribution.Models.Database.Entities;
using System;

namespace DigitalDistribution.Models.Responses.Library
{
    public class LibraryResponse
    {
        public string Licence { get; set; }
        public string DownloadLink { get; set; }
        public DateTime DateAdded { get; set; }
        public ProductEntity Product { get; set; }
    }
}
