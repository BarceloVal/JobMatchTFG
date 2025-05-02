using Job_Match.API.Models.Dtos;
using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user_provider")]
    public class m_user_providerController : ControllerBase
    {
        private readonly IMProviderService _providerService;
        private readonly job_match_context _context;

        public m_user_providerController(IMProviderService providerService, job_match_context context)
        {
            _providerService = providerService;
            _context = context;
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
                MapCoordinates = provider.map_coordinates
            };

            return Ok(dto);
        }

        [HttpGet("all-with-services")]
        public async Task<ActionResult<List<ProviderWithServicesDto>>> GetAllProvidersWithServices()
        {
            var providers = await _context.m_user_providers
            .Include(p => p.id_postal_codeNavigation)
            .Include(p => p.m_service_providers)
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
                Services = p.m_service_providers.Select(sp => new ServiceDto
                {
                    Id = sp.m_service.id,
                    Name = sp.m_service.name,
                    IdServiceType = sp.m_service.id_service_type,
                    ServiceTypeName = sp.m_service.id_service_typeNavigation.name
                }).ToList()
            }).ToList();

            return Ok(result);
        }


    }
}
