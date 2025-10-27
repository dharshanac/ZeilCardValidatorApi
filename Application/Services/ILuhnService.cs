namespace ZeilCardValidatorApi.Application.Services
{
    public interface ILuhnService
    {
        /// <summary>
        /// Validates the supplied card number using the Luhn algorithm.
        /// Input can contain digits, spaces, or dashes. Returns false for malformed input (non-digit characters after stripping spaces/dashes) or invalid length.
        /// </summary>
        bool Validate(string rawCardNumber);


        /// <summary>
        /// Masks a card number for safe logging/display. Example: ************1234
        /// If the input is shorter than 4 digits, replaces all digits with '*'.
        /// </summary>
        string Mask(string rawCardNumber);
    }
}

