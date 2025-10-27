using FluentValidation;
using ZeilCardValidatorApi.Application.DTOs;

namespace ZeilCardValidatorApi.Application.Validators
{
    public class CreditCardRequestValidator : AbstractValidator<ValidateRequest>
    {
        public CreditCardRequestValidator()
        {
            RuleFor(x => x.CardNumber)
                    .NotEmpty().WithMessage("Card number is required.")
                    .Length(13, 19).WithMessage("Card number must be between 13 and 19 digits.")
                    .Must(card =>
                    {
                        var digitsOnly = card.Replace(" ", "");
                        return digitsOnly.All(char.IsDigit);
                    }).WithMessage("Card number must contain digits only.");
        }
    }
}
