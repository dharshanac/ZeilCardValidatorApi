using System.Text.RegularExpressions;
using ZeilCardValidatorApi.Application.Extensions;

namespace ZeilCardValidatorApi.Application.Services
{
    public class LuhnService : ILuhnService
    {
        public bool Validate(string rawCardNumber)
        {
            if (string.IsNullOrWhiteSpace(rawCardNumber))
                return false;

            int sum = 0;
            bool alternate = false;
            var digitsOnly = rawCardNumber.NormalizeCardNumber();

            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(digitsOnly[i]))
                    return false;

                int n = digitsOnly[i] - '0';

                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        public string Mask(string rawCardNumber)
        {
            if (string.IsNullOrWhiteSpace(rawCardNumber))
                return string.Empty;

            var digitsOnly = rawCardNumber.NormalizeCardNumber();
            var length = digitsOnly.Length;
            if (length == 0)
                return string.Empty;

            if (length <= 4)
                return new string('*', length);

            var visible = digitsOnly.Substring(length - 4);
            return new string('*', length - 4) + visible;
        }
    }
}
