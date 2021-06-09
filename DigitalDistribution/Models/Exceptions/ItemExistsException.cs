namespace DigitalDistribution.Models.Exceptions
{
    public class ItemExistsException:System.Exception
    {
        public ItemExistsException(string message) : base(message)
        {
        }
    }
}
