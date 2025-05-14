using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/offer-subscriptions")]
    public class t_offer_subscriptionController : ControllerBase
    {
        #region parameters

        private readonly IMOfferSubscriptionService _service;

        public t_offer_subscriptionController(IMOfferSubscriptionService service)
        {
            _service = service;
        }

        #endregion

        #region methods POST

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToOffer([FromBody] OfferSubscriptionDto dto)
        {
            try
            {
                var alreadySubscribed = await _service.IsAlreadySubscribedAsync(dto);

                if (alreadySubscribed)
                    return Conflict("Ya estás inscrito a esta oferta.");

                await _service.SubscribeToOfferAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR API] {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        #endregion

        #region methods GET

        [HttpGet("by-provider/{idUserProvider}")]
        public async Task<ActionResult<List<SubscribedOfferDto>>> GetOffersByProvider(Guid idUserProvider)
        {
            var result = await _service.GetOffersByProviderAsync(idUserProvider);
            return Ok(result);
        }

        #endregion+
    }
}
