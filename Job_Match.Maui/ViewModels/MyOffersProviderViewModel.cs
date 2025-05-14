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

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
