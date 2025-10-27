namespace ZeilCardValidatorApi.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        public string ErrorCode { get; }

        public DomainValidationException(string message, string errorCode = "DOMAIN_VALIDATION_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
