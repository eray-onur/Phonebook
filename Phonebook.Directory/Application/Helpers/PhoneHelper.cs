using System.Text.RegularExpressions;

namespace Phonebook.Directory.Application.Helpers
{
    public static class PhoneHelper
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$"); return phoneRegex.IsMatch(phoneNumber);
        }
    }
}
