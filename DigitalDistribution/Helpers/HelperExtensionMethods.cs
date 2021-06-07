using System;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
        private static Random random = new Random();
       
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string CreateLicence()
        {
            var licence = new StringBuilder();
            int count = 0;
            while (count < 4)
            {
                licence.Append(RandomString(4) + "-");
                count++;
            }
            licence.Remove(licence.Length - 1, 1);
            return licence.ToString();
        }
    }
}
