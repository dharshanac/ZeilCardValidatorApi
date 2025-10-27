namespace ZeilCardValidatorApi.Application.Services
{
    public interface ILuhnService
    {
        bool Validate(string rawCardNumber);
    }
}

