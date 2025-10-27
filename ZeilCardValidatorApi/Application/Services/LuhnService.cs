using System.Text.RegularExpressions;

namespace ZeilCardValidatorApi.Application.Services
{
    public class LuhnService : ILuhnService
    {
        public bool Validate(string rawCardNumber)
        {
            return true;
        }
    }
}
