using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class MyOffersProviderViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SubscribedOfferDto> MyOffers { get; set; } = new();

        private readonly Job_Match.Maui.ApiService.ApiService _api = new();

        public async Task LoadMyOffersAsync(Guid idUserProvider)
        {
            var result = await _api.GetOffersByProviderAsync(idUserProvider);
            MyOffers.Clear();
            foreach (var offer in result)
                MyOffers.Add(offer);
        }

        public async Task QuitOfferAsync(Guid idProvider, Guid idTaskRequest)
        {
            var dto = new QuitOfferDto
            {
                IdUserProvider = idProvider,
                IdTaskRequest = idTaskRequest
            };

            await _api.PostAsync("api/offer-subscriptions/quit", dto);
            await LoadMyOffersAsync(idProvider); // Recargar después de quitar
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
