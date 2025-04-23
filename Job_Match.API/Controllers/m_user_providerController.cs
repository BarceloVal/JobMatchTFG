using Job_Match.API.Models.Dtos;
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


        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<ProfileProviderDto>> GetByMail(string mail)
        {
            var provider = await _providerService.GetProviderByMailAsync(mail);

            if (provider == null)
                return NotFound();

            var dto = new ProfileProviderDto
            {
                FirstName = provider.id_userNavigation.first_name,
                LastName = provider.id_userNavigation.last_name,
                TradeName = provider.trade_name,
                Mail = provider.mail,
                Password = provider.id_userNavigation.password,
                IdDocType = provider.id_doc_type,
                Document = provider.num_doc,
                Website = provider.web_site,
                Address = provider.address,
                IdPostalCode = provider.id_postal_code,
                Phone1 = provider.phone_number_1,
                Phone2 = provider.phone_number_2,
                Description = provider.description,
                MapCoordinates = provider.map_coordinates
            };

            return Ok(dto);
        }
    }
}
