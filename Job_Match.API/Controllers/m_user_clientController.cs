using Job_Match.API.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user_client")]
    public class m_user_clientController : ControllerBase
    {
        private readonly IMClientService _clientService;

        public m_user_clientController(IMClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateClientDto dto)
        {
            var result = await _clientService.CreateClientAsync(dto);
            if (!result)
                return BadRequest("Error al crear el usuario.");
            return Ok();
        }
    }
}
