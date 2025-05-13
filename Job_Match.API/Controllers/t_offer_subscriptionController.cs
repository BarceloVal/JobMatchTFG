using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
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
            await _service.SubscribeToOfferAsync(dto);
            return Ok();
        }

        #endregion
    }
}
