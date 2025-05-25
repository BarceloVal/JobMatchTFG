using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Job_Match.Maui.ViewModels
{
    public class ProfileUserScoreViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Job_Match.Maui.ApiService.ApiService _apiService = new();

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

        public bool IsProvider { get => _isProvider; set { _isProvider = value; OnPropertyChanged(nameof(IsProvider)); } }
        private bool _isProvider;

        public Guid IdClientUser { get; set; }
        public Guid IdProviderUser { get; set; }

        public ICommand GoToProviderProfileCommand { get; }

        public ObservableCollection<TaskScoreDisplayDto> ReceivedScores { get; set; } = new();

        public ProfileUserScoreViewModel()
        {
            GoToProviderProfileCommand = new Command(async () => await OnGoToProviderProfile());
        }

        public async Task LoadUserProfileAsync(Guid userId)
        {
            try
            {
                try
                {
                    Guid providerId = await _apiService.GetProviderIdFromUserIdAsync(userId);
                    var provider = await _apiService.GetProviderByIdAsync(providerId);
                    if (provider != null)
                    {
                        IsProvider = true;
                        IdProviderUser = provider.IdUser;
                        IdClientUser = UserSessionService.Instance.Id;
                        DisplayName = $"{provider.FirstName} {provider.LastName}";
                        Mail = provider.Mail;
                        Phone1 = provider.Phone1;
                        Phone2 = provider.Phone2;
                        PostalCode = provider.PostalCodeName;
                        TradeName = provider.TradeName;
                        Website = provider.Website;
                        Description = provider.Description;

                        if (provider.Services != null && provider.Services.Any())
                        {
                            ServiceType = provider.Services[0].ServiceTypeName;
                            Service = provider.Services[0].ServiceName;
                        }

                        var scores = await _apiService.GetScoresReceivedByUserAsync(userId);
                        ReceivedScores.Clear();
                        foreach (var score in scores)
                        {
                            score.IsMine = score.IdUserReviewer == UserSessionService.Instance.Id;
                            ReceivedScores.Add(score);
                        }

                        return;
                    }
                }
                catch { }

                var client = await _apiService.GetClientProfileByUserIdAsync(userId);
                if (client != null)
                {
                    IsProvider = false;
                    IdClientUser = client.IdUser;
                    IdProviderUser = UserSessionService.Instance.Id;
                    DisplayName = $"{client.FirstName} {client.LastName}";
                    Mail = client.Mail;
                    Phone1 = client.Phone1;
                    Phone2 = client.Phone2;
                    PostalCode = client.PostalCodeName;

                    var scores = await _apiService.GetScoresReceivedByUserAsync(userId);
                    ReceivedScores.Clear();
                    foreach (var score in scores)
                    {
                        score.IsMine = score.IdUserReviewer == UserSessionService.Instance.Id;
                        ReceivedScores.Add(score);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general en LoadUserProfileAsync: {ex.Message}");
            }
        }

        private async Task OnGoToProviderProfile()
        {
            var myUserId = UserSessionService.Instance.Id;
            var targetUserId = (myUserId == IdClientUser) ? IdProviderUser : IdClientUser;

            if (targetUserId != Guid.Empty)
            {
                await Shell.Current.GoToAsync($"ScoreUser?providerId={targetUserId}");
            }
        }

        public async Task<bool> DeleteScoreAsync(TaskScoreDisplayDto score)
        {
            var success = await _apiService.DeactivateScoreAsync(score.IdUserReviewer, score.IdUserReviewed);
            return success;
        }

    }
}
