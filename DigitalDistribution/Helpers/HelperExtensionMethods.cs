using System.Linq;
using System.Security.Claims;

namespace DigitalDistribution.Helpers
{
    public static class HelperExtensionMethods
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.Claims
                .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return 0;

            return userId.ToInt();
        }

        public static int ToInt(this string obj)
        {
            return int.Parse(obj);
        }
    }
}
