namespace ZeilCardValidatorApi.Application.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeCardNumber(this string cardNumber)
        {
            return new string(cardNumber.Where(c => c != ' ' && c != '-').ToArray());
        }
    }
}
