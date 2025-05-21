using Job_Match.API.Models.Dtos;
using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user_client")]
    public class m_user_clientController : ControllerBase
    {
        #region parameters

        private readonly IMClientService _clientService;
        private readonly job_match_context _context;

        public m_user_clientController(IMClientService clientService, job_match_context context)
        {
            _clientService = clientService;
            _context = context;
        }

        #endregion

        #region methods POST

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateClientDto dto)
        {
            var result = await _clientService.CreateClientAsync(dto);
            if (!result)
                return BadRequest("Error al crear el usuario.");
            return Ok();
        }

        [HttpPost("change-provider")]
        public async Task<IActionResult> ChangeClientToProvider([FromBody] ChangeProviderToClientDto dto)
        {
            var success = await _clientService.ChangeProviderToClientAsync(dto);
            if (!success)
                return BadRequest("Error al cambiar de proveedor a cliente.");

            return Ok();
        }

        #endregion

        #region methods GET

        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<ProfileClientDto>> GetByMail(string mail)
        {
            var client = await _clientService.GetClientByMailAsync(mail);

            if (client == null)
                return NotFound();

            var dto = new ProfileClientDto
            {
                Id = client.id,
                IdUser = client.id_user,
                FirstName = client.id_userNavigation.first_name,
                LastName = client.id_userNavigation.last_name,
                Mail = client.mail,
                IdDocType = client.id_doc_type,
                Document = client.num_doc,
                Address = client.address,
                IdPostalCode = client.id_postal_code,
                PostalCodeName = $"{client.id_postal_codeNavigation.cod_auto_name}/{client.id_postal_codeNavigation.cod_pro_name}/{client.id_postal_codeNavigation.cod_loc_name}",
                Phone1 = client.phone_number_1,
                Phone2 = client.phone_number_2
            };

            return Ok(dto);
        }

        [HttpGet("exists/{idUser}")]
        public async Task<IActionResult> Exists(Guid idUser)
        {
            var exists = await _clientService.ClientExistsAsync(idUser);
            return exists ? Ok() : NotFound();
        }

        [HttpGet("profile/{idUser}")]
        public async Task<ActionResult<ProfileClientDto>> GetProfile(Guid idUser)
        {
            var profile = await _clientService.GetClientProfileAsync(idUser);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpGet("active-by-client/{idUserClient}")]
        public async Task<ActionResult<List<TaskRequestWithProvidersDto>>> GetActiveOffersByClient(Guid idUserClient)
        {
            var tasks = await _context.t_task_requests
                .Where(t => t.id_user_client == idUserClient && t.active)
                .Include(t => t.id_statusNavigation)
                .Include(t => t.id_serviceNavigation)
                    .ThenInclude(s => s.id_service_typeNavigation)
                .Include(t => t.id_postal_codeNavigation)
                .ToListAsync();

            var result = new List<TaskRequestWithProvidersDto>();

            foreach (var t in tasks)
            {
                var providers = await _context.t_offer_subscriptions
                    .Where(os => os.id_task_request == t.id && os.active)
                    .Include(os => os.id_user_providerNavigation)
                        .ThenInclude(p => p.id_postal_codeNavigation)
                    .Include(os => os.id_user_providerNavigation.id_userNavigation)
                    .Select(os => new ProviderTaskRequestDto
                    {
                        IdUser = os.id_user_providerNavigation.id_user,
                        FullName = os.id_user_providerNavigation.id_userNavigation.first_name + " " +
                                   os.id_user_providerNavigation.id_userNavigation.last_name,
                        TradeName = os.id_user_providerNavigation.trade_name,
                        Address = os.id_user_providerNavigation.address,
                        PostalCodeName = os.id_user_providerNavigation.id_postal_codeNavigation.cod_auto_name + "/" +
                                         os.id_user_providerNavigation.id_postal_codeNavigation.cod_pro_name + "/" +
                                         os.id_user_providerNavigation.id_postal_codeNavigation.cod_loc_name,
                        Mail = os.id_user_providerNavigation.mail,
                        Phone1 = os.id_user_providerNavigation.phone_number_1,
                        Phone2 = os.id_user_providerNavigation.phone_number_2,
                        Description = os.id_user_providerNavigation.description,
                        IdTaskRequest = t.id
                    })
                    .ToListAsync();

                result.Add(new TaskRequestWithProvidersDto
                {
                    Id = t.id,
                    Title = t.title,
                    Description = t.description,
                    Status = t.id_statusNavigation.name,
                    Service = t.id_serviceNavigation.name,
                    ServiceType = t.id_serviceNavigation.id_service_typeNavigation.name,
                    PostalCodeName = $"{t.id_postal_codeNavigation.cod_auto_name}/{t.id_postal_codeNavigation.cod_pro_name}/{t.id_postal_codeNavigation.cod_loc_name}",
                    DateStart = t.date_start,
                    DateEnd = t.date_end,
                    Active = t.active,
                    Providers = providers
                });
            }

            return Ok(result);
        }

        #endregion

        #region methods PUT

        [HttpPut]
        public async Task<IActionResult> UpdateProfileClient([FromBody] UpdateProfileClientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _clientService.UpdateClientProfileAsync(dto);
            if (!success)
                return StatusCode(500, "Error al actualizar el perfil de cliente");

            return NoContent();
        }

        #endregion
    }
}