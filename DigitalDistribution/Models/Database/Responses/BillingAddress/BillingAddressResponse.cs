namespace DigitalDistribution.Models.Database.Responses.BillingAddress
{
    public class BillingAddressResponse:BaseResponse
    {
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAdress { get; set; }
    }
}
