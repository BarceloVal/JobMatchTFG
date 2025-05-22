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

            var myUserId = UserSessionService.Instance.Id;
            var isClient = myUserId == idClientUser;

            if (isClient)
            {
                // Soy cliente, el proveedor ya es el id_user → obtengo el nombre
                var providerDto = await _apiService.GetProviderByUserIdAsync(idProvider);
                if (providerDto != null)
                    ProviderName = $"{providerDto.FirstName} {providerDto.LastName}";
            }
            else
            {
                // Soy proveedor: idClientUser es el id del cliente, necesito obtener el id_user
                var idUserClient = await _apiService.GetUserIdFromClientIdAsync(idClientUser);
                if (idUserClient.HasValue)
                {
                    var clientDto = await _apiService.GetClientProfileByUserIdAsync(idUserClient.Value);
                    if (clientDto != null)
                        ProviderName = $"{clientDto.FirstName} {clientDto.LastName}";

                    idClientUser = idUserClient.Value; // corregir para el fetch de mensajes
                    IdClientUser = idUserClient.Value;
                }

                // Además: necesito el id_user del proveedor, ya que el parámetro es el id del proveedor
                var providerDto = await _apiService.GetProviderByIdAsync(idProvider);
                if (providerDto != null)
                {
                    idProvider = providerDto.IdUser; // corregir para el fetch de mensajes
                    IdProviderUser = providerDto.IdUser;
                }
            }

            // Obtener los mensajes con los id_user reales
            var messages = await _apiService.GetMessagesBetweenUsersAsync(idClientUser, idProvider);
            Messages.Clear();

            foreach (var msg in messages)
            {
                msg.IsMine = msg.IdSender == myUserId;
                Messages.Add(msg);
            }

            // Esperar un poco y luego hacer scroll
            await Task.Delay(100); // Espera para asegurar que se rendericen los mensajes

            await _apiService.MarkMessagesAsReadAsync(myUserId, isClient ? idProvider : idClientUser);
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            var myUserId = UserSessionService.Instance.Id;

            // Determinar el receptor correctamente
            Guid receiverId;
            bool isClient = myUserId == IdClientUser;

            if (isClient)
            {
                // Soy cliente, el proveedor es el receptor
                receiverId = IdProviderUser;
            }
            else
            {
                // Soy proveedor, el cliente es el receptor
                receiverId = IdClientUser;
            }

            // 1. Crear contacto si no existe
            var contactDto = new EnsureContactDto
            {
                IdUserClient = IdClientUser,
                IdUserProvider = IdProviderUser,
                IdTaskRequest = IdTaskRequest
            };

            await _apiService.PostAsync("api/task-contacts/ensure", contactDto);

            // 2. Enviar el mensaje
            var dto = new SendMessageDto
            {
                IdSender = myUserId,
                IdReceiver = receiverId,
                Message = NewMessage,
                IdTaskRequest = IdTaskRequest
            };

            var success = await _apiService.SendMessageAsync(dto);
            if (success)
            {
                Messages.Add(new ChatMessageDto
                {
                    IdSender = myUserId,
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
