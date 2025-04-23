using Job_Match.API.Models.Dtos;
using Job_Match.Data.Models.Dtos;
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

        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<ProfileClientDto>> GetByMail(string mail)
        {
            var client = await _clientService.GetClientByMailAsync(mail);

            if (client == null)
                return NotFound();

            var dto = new ProfileClientDto
            {
                FirstName = client.id_userNavigation.first_name,
                LastName = client.id_userNavigation.last_name,
                Mail = client.mail,
                IdDocType = client.id_doc_type,
                Document = client.num_doc,
                Address = client.address,
                IdPostalCode = client.id_postal_code,
                Phone1 = client.phone_number_1,
                Phone2 = client.phone_number_2
            };

            return Ok(dto);
        }
    }
}
