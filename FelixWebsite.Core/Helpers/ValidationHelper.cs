
using System.Text.RegularExpressions;

namespace FelixWebsite.Core.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(this string tel)
        {
            return Regex.Match(tel, @"^((\+|00)32\s?|0)(\d\s?\d{3}|\d{2}\s?\d{2})(\s?\d{2}){2}$").Success;
        }
    }
}
