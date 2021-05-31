namespace DigitalDistribution.Models.Database.Requests.BillingAddress
{
    public class UpdateBillingAddress
    {
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAdress { get; set; }
    }
}
