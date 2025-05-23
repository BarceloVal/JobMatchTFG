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
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private string _displayName;


        private readonly Job_Match.Maui.ApiService.ApiService _apiService = new();

        public async Task LoadUserProfileAsync(Guid userId)
        {
            // 1. Intentar cargar como proveedor
            var provider = await _apiService.GetProviderByUserIdAsync(userId);
            if (provider != null)
            {
                DisplayName = $"{provider.FirstName} {provider.LastName}";
                return;
            }

            // 2. Si no es proveedor, intentar cargar como cliente
            var client = await _apiService.GetClientProfileByUserIdAsync(userId);
            if (client != null)
            {
                DisplayName = $"{client.FirstName} {client.LastName}";
                return;
            }

            DisplayName = "Usuario desconocido";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
