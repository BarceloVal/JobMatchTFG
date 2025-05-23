using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class ProfileUserScoreViewModel : INotifyPropertyChanged
    {
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(nameof(DisplayName)); } }
        private string _displayName;

        public string Mail { get => _mail; set { _mail = value; OnPropertyChanged(nameof(Mail)); } }
        private string _mail;

        public string Phone1 { get => _phone1; set { _phone1 = value; OnPropertyChanged(nameof(Phone1)); } }
        private string _phone1;

        public string Phone2 { get => _phone2; set { _phone2 = value; OnPropertyChanged(nameof(Phone2)); } }
        private string _phone2;

        public string PostalCode { get => _postalCode; set { _postalCode = value; OnPropertyChanged(nameof(PostalCode)); } }
        private string _postalCode;

        public string TradeName { get => _tradeName; set { _tradeName = value; OnPropertyChanged(nameof(TradeName)); } }
        private string _tradeName;

        public string Website { get => _website; set { _website = value; OnPropertyChanged(nameof(Website)); } }
        private string _website;

        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
        private string _description;

        public string ServiceType { get => _serviceType; set { _serviceType = value; OnPropertyChanged(nameof(ServiceType)); } }
        private string _serviceType;

        public string Service { get => _service; set { _service = value; OnPropertyChanged(nameof(Service)); } }
        private string _service;


        private readonly Job_Match.Maui.ApiService.ApiService _apiService = new();

        public async Task LoadUserProfileAsync(Guid userId)
        {
            try
            {
                // 1. Obtener el ID del proveedor a partir del id_user
                Guid providerId = await _apiService.GetProviderIdFromUserIdAsync(userId);

                // 2. Obtener el perfil completo del proveedor usando ese ID
                var provider = await _apiService.GetProviderByIdAsync(providerId);
                if (provider != null)
                {
                    DisplayName = $"{provider.FirstName} {provider.LastName}";
                    Mail = provider.Mail;
                    Phone1 = provider.Phone1;
                    Phone2 = provider.Phone2;
                    PostalCode = provider.PostalCodeName;
                    TradeName = provider.TradeName;
                    Website = provider.Website;
                    Description = provider.Description;

                    // Verificamos si tiene servicios asociados
                    if (provider.Services != null && provider.Services.Any())
                    {
                        ServiceType = provider.Services[0].ServiceTypeName;
                        Service = provider.Services[0].ServiceName;
                    }

                    return;
                }

                // 3. Si no es proveedor, buscar como cliente
                var client = await _apiService.GetClientProfileByUserIdAsync(userId);
                if (client != null)
                {
                    DisplayName = $"{client.FirstName} {client.LastName}";
                    Mail = client.Mail;
                    Phone1 = client.Phone1;
                    Phone2 = client.Phone2;
                    PostalCode = client.PostalCodeName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en LoadUserProfileAsync: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
