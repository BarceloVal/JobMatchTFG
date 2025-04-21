using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user_provider")]
    public class m_user_providerController : ControllerBase
    {
        private readonly IMProviderService _providerService;

        public m_user_providerController(IMProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProviderDto dto)
        {
            var result = await _providerService.CreateProviderAsync(dto);
            if (!result)
                return BadRequest("Error al crear el proveedor.");
            return Ok();
        }
    }

}
