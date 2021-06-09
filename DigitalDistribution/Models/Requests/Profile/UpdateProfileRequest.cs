namespace DigitalDistribution.Models.Database.Requests.Profile
{
    public class UpdateProfileRequest
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
