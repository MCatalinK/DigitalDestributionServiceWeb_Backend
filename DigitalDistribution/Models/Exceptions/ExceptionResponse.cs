using Newtonsoft.Json;

namespace DigitalDistribution.Models.Exceptions
{
    public class ExceptionResponse
    {
        public string Message { get; set; }
        public string Type { get; set; }
        [JsonIgnore] public string Trace { get; set; }
    }
}
