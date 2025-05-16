using System.Collections.ObjectModel;
using System.Windows.Input;
using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using System.ComponentModel;
using System.Net.Sockets;

namespace Job_Match.MAUI.ViewModels
{
    public class ChatConversationViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();
        public ObservableCollection<ChatMessageDto> Messages { get; set; } = new();

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public ICommand SendMessageCommand { get; }

        // Reemplazá esto por lo que recibas desde navegación
        public Guid IdClientUser { get; set; }
        public Guid IdProviderUser { get; set; }

        public ChatConversationViewModel()
        {
            SendMessageCommand = new Command(async () => await SendMessageAsync());
        }

        public async Task LoadMessagesAsync(Guid idClientUser, Guid idProviderUser)
        {
            Console.WriteLine($"[LoadMessagesAsync] Cliente: {idClientUser} - Proveedor: {idProviderUser}");

            IdClientUser = idClientUser;
            IdProviderUser = idProviderUser;

            var list = await _apiService.GetMessagesBetweenUsersAsync(idClientUser, idProviderUser);

            Messages.Clear();
            foreach (var msg in list)
            {
                msg.IsMine = msg.IdSender == idClientUser; // o comparalo con UserSessionService.Instance.Id si preferís
                Messages.Add(msg);
            }
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            var dto = new SendMessageDto
            {
                IdSender = UserSessionService.Instance.Id, // ID del usuario cliente logueado
                IdReceiver = IdProviderUser,               // ID del proveedor con quien habla
                Message = NewMessage
            };

            var success = await _apiService.SendMessageAsync(dto);
            if (success)
            {
                Messages.Add(new ChatMessageDto
                {
                    IdSender = dto.IdSender,
                    Message = NewMessage,
                    SentOn = DateTime.Now,
                    IsMine = true,
                    SenderName = "Tú"
                });

                NewMessage = string.Empty;
            }
        }

        public async void Initialize(Guid idClient, Guid idProvider)
        {
            await LoadMessagesAsync(idClient, idProvider);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
