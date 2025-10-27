using System.Text.RegularExpressions;

namespace ZeilCardValidatorApi.Application.Services
{
    public class LuhnService : ILuhnService
    {
        // Remove spaces and dashes
        private static readonly Regex _cleanup = new("[\\s-]+", RegexOptions.Compiled);

        public bool Validate(string rawCardNumber)
        {
            if (string.IsNullOrWhiteSpace(rawCardNumber))
                return false;

            int sum = 0;
            bool alternate = false;
            var digitsOnly = _cleanup.Replace(rawCardNumber, string.Empty);

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
    }
}
