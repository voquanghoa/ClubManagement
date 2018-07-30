using System.Linq;

namespace ClubManagement.Ultilities
{
    public static class StringExtensions
    {
        public static bool IsValidEmailFormat(this string email)
        {
            var keys = new[] { "+", "&&", "||", "!", "(", ")", "{", "}", "[", "]", "^", "~", "*", "?", ":", "\\", "\"" };

            if (email.Count(x => x == '@') == 1)
            {
                var emailName = email.Split('@')[0] ?? "";
                var domainName = email.Split('@')[1] ?? "";

                if (string.IsNullOrEmpty(emailName) || string.IsNullOrEmpty(domainName)) return false;

                if (keys.Any(x => emailName.Contains(x))) return false;

                if (domainName.Count(x => x == '.') > 0
                    && !domainName.Contains("..")
                    && domainName[0] != '.'
                    && domainName[domainName.Length - 1] != '.') return true;

                return false;
            }

            return false;
        }
    }
}