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

        [HttpPost]
        public IActionResult Post([FromBody] ValidateRequest request)
        {
            var isValid = _luhnService.Validate(request.CardNumber);
            return Ok(new { request.CardNumber, IsValid = isValid });
        }
    }
}

