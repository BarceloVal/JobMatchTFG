using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;


namespace Job_Match.Maui.ViewModels
{
    public class OfferClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly ApiServiceClass _apiService = new();

        public ObservableCollection<TaskRequestWithProvidersDto> ActiveOffers { get; set; } = new();

        public async Task LoadActiveOffersAsync(Guid clientId)
        {
            try
            {
                ActiveOffers.Clear();

                var offers = await _apiService.GetActiveOffersByClientAsync(clientId);
                if (offers != null && offers.Count > 0)
                {
                    var sortedOffers = offers.OrderByDescending(o => o.DateStart);

                    foreach (var offer in sortedOffers)
                    {
                        ActiveOffers.Add(offer);
                    }
                }
                else
                {
                    Console.WriteLine("No hay ofertas activas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar ofertas activas: {ex.Message}");
            }
        }
    }

}
