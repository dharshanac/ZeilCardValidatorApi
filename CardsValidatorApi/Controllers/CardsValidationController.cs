using Microsoft.AspNetCore.Mvc;
using ZeilCardValidatorApi.Application.DTOs;
using ZeilCardValidatorApi.Application.Services;

namespace ZeilCardValidatorApi.CardsValidatorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsValidationController : ControllerBase
    {
        private readonly ILuhnService _luhnService;
        private readonly ILogger<CardsValidationController> _logger;

        public CardsValidationController(ILuhnService luhnService, ILogger<CardsValidationController> logger)
        {
            _luhnService = luhnService;
            _logger = logger;
        }

        /// <summary>
        /// Validate a credit card number using the Luhn algorithm.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// POST /api/validate
        /// {
        /// "cardNumber": "4242 4242 4242 4242"
        /// }
        /// </remarks>
        /// <param name="request">Request body containing the card number.</param>
        /// <returns>JSON payload indicating whether the card number passes Luhn.
        /// The response will not contain the full card number. Only a masked version is returned for display.</returns>
        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostValidate([FromBody] ValidateRequest request)
        {
            var masked = _luhnService.Mask(request.CardNumber);

            // IMPORTANT: Do not log full card numbers. Log masked only.
            _logger.LogInformation("Validating card number {CardMasked}", masked);

            var isValid = _luhnService.Validate(request.CardNumber);
            return Ok(new { CardNumber = masked, IsValid = isValid });
        }
    }
}

