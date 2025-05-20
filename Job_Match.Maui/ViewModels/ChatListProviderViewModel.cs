using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Job_Match.Maui.ViewModels
{
    public class ChatListProviderViewModel : INotifyPropertyChanged
    {
        private readonly Job_Match.Maui.ApiService.ApiService _apiService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<ChatSummaryDto> Chats { get; set; } = new();

        public ICommand LoadChatsCommand { get; }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ChatListProviderViewModel()
        {
            _apiService = new Job_Match.Maui.ApiService.ApiService();
            LoadChatsCommand = new Command(async () => await LoadChatsAsync());
            LoadChatsCommand.Execute(null); // carga inicial
        }

        public async Task LoadChatsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Chats.Clear();

                var idProvider = UserSessionService.Instance.IdUserProvider ?? Guid.Empty;
                var response = await _apiService.GetAsync<List<ChatSummaryDto>>($"api/task-contacts/provider/{idProvider}");

                if (response != null)
                {
                    foreach (var chat in response)
                        Chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
