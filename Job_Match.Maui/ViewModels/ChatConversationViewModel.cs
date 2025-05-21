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

        private string _providerName;
        public string ProviderName
        {
            get => _providerName;
            set
            {
                _providerName = value;
                OnPropertyChanged(nameof(ProviderName));
            }
        }

        public Guid? IdTaskRequest { get; set; }

        public ICommand SendMessageCommand { get; }

        // Reemplazá esto por lo que recibas desde navegación
        public Guid IdClientUser { get; set; }
        public Guid IdProviderUser { get; set; }

        public ChatConversationViewModel()
        {
            SendMessageCommand = new Command(async () => await SendMessageAsync());
        }

        public async Task LoadMessagesAsync(Guid idClientUser, Guid idProvider, Guid idTaskRequest)
        {
            Console.WriteLine($"[LoadMessagesAsync] Cliente: {idClientUser} - Proveedor (id): {idProvider}");

            IdClientUser = idClientUser;
            IdProviderUser = idProvider;

            if (idTaskRequest != Guid.Empty)
                IdTaskRequest = idTaskRequest;

            // 1. Obtener el ProfileProviderDto desde el ID del proveedor
            var providerDto = await _apiService.GetProviderByUserIdAsync(idProvider);

            if (providerDto != null)
            {
                ProviderName = $"{providerDto.FirstName} {providerDto.LastName}";
                Console.WriteLine($"PROVIDER NAME: {ProviderName}");

                // 2. Obtener el ID del usuario (m_user) del proveedor
                IdProviderUser = providerDto.IdUser;
            }
            else
            {
                Console.WriteLine("No se encontró el proveedor.");
            }

            // 3. Obtener los mensajes usando los ID de usuario
            var list = await _apiService.GetMessagesBetweenUsersAsync(idClientUser, IdProviderUser);

            Messages.Clear();
            foreach (var msg in list)
            {
                msg.IsMine = msg.IdSender == idClientUser;
                Messages.Add(msg);
            }
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            // 1. Crear contacto si no existe
            var contactDto = new EnsureContactDto
            {
                IdUserClient = UserSessionService.Instance.Id,
                IdUserProvider = IdProviderUser,
                //IdTaskRequest = IdTaskRequest.HasValue && IdTaskRequest != Guid.Empty ? IdTaskRequest : null
                IdTaskRequest = IdTaskRequest
            };

            await _apiService.PostAsync("api/task-contacts/ensure", contactDto);

            // 2. Enviar el mensaje
            var dto = new SendMessageDto
            {
                IdSender = UserSessionService.Instance.Id, // ID del cliente logueado
                IdReceiver = IdProviderUser,
                Message = NewMessage,
                IdTaskRequest = IdTaskRequest
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
                    SenderName = "Tú",
                    IdTaskRequest = dto.IdTaskRequest
                });

                NewMessage = string.Empty;
                ChatNavigationState.HasSentNewMessage = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Clase auxiliar
    public static class ChatNavigationState
    {
        public static bool HasSentNewMessage { get; set; } = false;
    }

}
