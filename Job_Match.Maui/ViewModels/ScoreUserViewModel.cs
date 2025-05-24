using Job_Match.Maui.ApiService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Job_Match.Maui.ViewModels
{
    public class ScoreUserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Job_Match.Maui.ApiService.ApiService _apiService = new();

        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(nameof(DisplayName)); } }
        private string _displayName;

        public async Task LoadUserScoreAsync(Guid userId)
        {
            try
            {
                try
                {
                    Guid providerId = await _apiService.GetProviderIdFromUserIdAsync(userId);
                    var provider = await _apiService.GetProviderByIdAsync(providerId);
                    if (provider != null)
                    {
                        DisplayName = $"{provider.FirstName} {provider.LastName}";
                        return;
                    }
                }
                catch { }

                var client = await _apiService.GetClientProfileByUserIdAsync(userId);
                if (client != null)
                {
                    DisplayName = $"{client.FirstName} {client.LastName}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general en LoadUserScoreAsync: {ex.Message}");
            }
        }

    }
}
