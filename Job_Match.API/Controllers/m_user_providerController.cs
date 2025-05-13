using Job_Match.API.Models.Dtos;
using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user_provider")]
    public class m_user_providerController : ControllerBase
    {
        #region parameters

        private readonly IMProviderService _providerService;
        private readonly job_match_context _context;

        public m_user_providerController(IMProviderService providerService, job_match_context context)
        {
            _providerService = providerService;
            _context = context;
        }

        #endregion

        #region methods POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProviderDto dto)
        {
            var result = await _providerService.CreateProviderAsync(dto);
            if (!result)
                return BadRequest("Error al crear el proveedor.");
            return Ok();
        }

        [HttpPost("change-client")]
        public async Task<IActionResult> ChangeClientToProvider([FromBody] ChangeClientToProviderDto dto)
        {
            var success = await _providerService.ChangeClientToProviderAsync(dto);
            if (!success)
                return BadRequest("Error al cambiar de cliente a proveedor.");

            return Ok();
        }

        #endregion

        #region methods GET
        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<ProfileProviderDto>> GetByMail(string mail)
        {
            var provider = await _context.m_user_providers
                .Include(p => p.id_userNavigation)
                .Include(p => p.id_postal_codeNavigation)
                .Include(p => p.m_service_providers.Where(sp => sp.active))
                    .ThenInclude(sp => sp.m_service)
                        .ThenInclude(s => s.id_service_typeNavigation)
                .FirstOrDefaultAsync(p => p.mail == mail);

            if (provider == null)
                return NotFound();

            var dto = new ProfileProviderDto
            {
                IdProvider = provider.id,
                IdUser = provider.id_user,
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
                PostalCodeName = $"{provider.id_postal_codeNavigation.cod_auto_name}/{provider.id_postal_codeNavigation.cod_pro_name}/{provider.id_postal_codeNavigation.cod_loc_name}",
                Phone1 = provider.phone_number_1,
                Phone2 = provider.phone_number_2,
                Description = provider.description,
                MapCoordinates = provider.map_coordinates,
                Services = provider.m_service_providers.Select(sp => new ServiceDto
                {
                    Id = sp.m_service.id,
                    ServiceName = sp.m_service.name,
                    ServiceTypeName = sp.m_service.id_service_typeNavigation.name
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet("all-with-services")]
        public async Task<ActionResult<List<ProviderWithServicesDto>>> GetAllProvidersWithServices()
        {
            var providers = await _context.m_user_providers
                .Where(p => p.active)
                .Include(p => p.id_postal_codeNavigation)
                .Include(p => p.m_service_providers.Where(sp => sp.active))
                    .ThenInclude(sp => sp.m_service)
                        .ThenInclude(s => s.id_service_typeNavigation)
                .ToListAsync();


            var result = providers.Select(p => new ProviderWithServicesDto
            {
                Id = p.id,
                TradeName = p.trade_name,
                PhoneNumber1 = p.phone_number_1,
                PhoneNumber2 = p.phone_number_2,
                Mail = p.mail,
                Website = p.web_site,
                Description = p.description,
                IdPostalCode = p.id_postal_code,
                PostalCodeName = $"{p.id_postal_codeNavigation.cod_auto_name}/{p.id_postal_codeNavigation.cod_pro_name}/{p.id_postal_codeNavigation.cod_loc_name}",
                Services = p.m_service_providers
            .Where(sp => sp.active && sp.m_service.active) // Extra seguro
            .Select(sp => new ServiceDto
            {
                Id = sp.m_service.id,
                Name = sp.m_service.name,
                IdServiceType = sp.m_service.id_service_type,
                ServiceName = sp.m_service.name,
                ServiceTypeName = sp.m_service.id_service_typeNavigation.name
            }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("exists/{idUser}")]
        public async Task<IActionResult> Exists(Guid idUser)
        {
            var exists = await _providerService.ProviderExistsAsync(idUser);
            return exists ? Ok() : NotFound();
        }

        #endregion

        #region methods PUT

        [HttpPut]
        public async Task<IActionResult> UpdateProfileProvider([FromBody] UpdateProfileProviderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _providerService.UpdateProviderProfileAsync(dto);
            if (!success)
                return StatusCode(500, "Error al actualizar el perfil de proveedor");

            return NoContent();
        }

        #endregion
    }
}
