using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_service_provider")]
    public class m_service_providerController : ControllerBase
    {
        #region parameters

        private readonly IMServiceProviderService _serviceProviderService;

        public m_service_providerController(IMServiceProviderService serviceProviderService)
        {
            _serviceProviderService = serviceProviderService;
        }

        #endregion

        #region methods GET

        [HttpGet]
        public async Task<ActionResult<List<m_service_provider>>> GetAll()
        {
            var providers = await _serviceProviderService.GetAllServiceProvidersAsync();
            return Ok(providers);
        }

        #endregion

        #region methods POST
        [HttpPost]
        public async Task<IActionResult> AddServicesToProvider([FromBody] ServiceProviderDto dto)
        {
            await _serviceProviderService.AddServicesToProviderAsync(dto.ProviderId, dto.ServiceIds);
            return Ok();
        }

        #endregion
    }
}
