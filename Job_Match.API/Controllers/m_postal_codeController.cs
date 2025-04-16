using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Job_Match.Data.Models;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/postal-codes")]
    public class m_postal_codeController : ControllerBase
    {
        private readonly IMPostalCodeService _postalCodeService;

        public m_postal_codeController(IMPostalCodeService postalCodeService)
        {
            _postalCodeService = postalCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var postalCodes = await _postalCodeService.GetAllAsStringAsync();
            return Ok(postalCodes);
        }
    }
}
