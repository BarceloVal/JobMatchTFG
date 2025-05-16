using Job_Match.Data.Models.Dtos;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Job_Match.Maui.ApiService;
using System.ComponentModel;
using Job_Match.Maui;

namespace Job_Match.MAUI.ViewModels
{
    public class ChatListClientViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

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

        public ChatListClientViewModel()
        {
            _apiService = new ApiService();
            LoadChatsCommand = new Command(async () => await LoadChatsAsync());
        }

        private async Task LoadChatsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Chats.Clear();

                var idClient = UserSessionService.Instance.IdUserClient ?? Guid.Empty;
                var response = await _apiService.GetAsync<List<ChatSummaryDto>>($"api/task-contacts/client/{idClient}");

                if (response != null)
                {
                    foreach (var chat in response)
                        Chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar errores o mostrar alertas
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
