using System.Linq;
using System.Text.RegularExpressions;

namespace ClubManagement.Ultilities
{
    public static class StringExtensions
    {
        public static bool IsValidEmailFormat(this string email)
        {
            var theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, theEmailPattern);
        }
    }
}