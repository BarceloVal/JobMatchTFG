using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMOfferSubscriptionService
    {
        Task SubscribeToOfferAsync(OfferSubscriptionDto dto);
        Task<bool> IsAlreadySubscribedAsync(OfferSubscriptionDto dto);
        Task<List<SubscribedOfferDto>> GetOffersByProviderAsync(Guid idUserProvider);
        Task<bool> QuitOfferAsync(QuitOfferDto dto);
    }
}
