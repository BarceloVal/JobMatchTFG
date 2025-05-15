using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Job_Match.Maui.ApiService
{
    public class UserSessionService : INotifyPropertyChanged
    {
        private static UserSessionService _instance;
        public static UserSessionService Instance => _instance ??= new UserSessionService();

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; private set; }
        public Guid? IdUserClient { get; private set; }
        public Guid? IdUserProvider { get; private set; }
        public string Mail { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsClient { get; private set; }
        public bool IsProvider { get; private set; }

        public string Token { get; private set; }
        public string? Password { get; internal set; }

        private ProfileClientDto _currentUserClient;
        public ProfileClientDto CurrentUserClient
        {
            get => _currentUserClient;
            set
            {
                if (_currentUserClient != value)
                {
                    _currentUserClient = value;
                    OnPropertyChanged();
                }
            }
        }

        private UserSessionService() { }

        public void Load(LoginResponse loginResponse, string plainPassword)
        {
            var user = loginResponse.user;

            Id = user.id;
            IdUserClient = user.id_user_client;
            IdUserProvider = user.id_user_provider;
            Mail = user.mail;
            Password = plainPassword;
            FirstName = user.first_name;
            LastName = user.last_name;
            IsClient = user.is_client;
            IsProvider = user.is_provider;
            Token = loginResponse.token;
        }

        public void UpdateClientProfile(UpdateProfileClientDto dto)
        {
            if (CurrentUserClient == null) return;

            CurrentUserClient.FirstName = dto.FirstName;
            CurrentUserClient.LastName = dto.LastName;
            CurrentUserClient.Document = dto.Document;
            CurrentUserClient.IdDocType = dto.IdDocType;
            CurrentUserClient.Address = dto.Address;
            CurrentUserClient.IdPostalCode = dto.IdPostalCode;
            CurrentUserClient.Phone1 = dto.Phone1;
            CurrentUserClient.Phone2 = dto.Phone2;

            // También actualiza las propiedades simples (opcional pero recomendado)
            FirstName = dto.FirstName;
            LastName = dto.LastName;

            // Notificar cambios del objeto completo
            OnPropertyChanged(nameof(CurrentUserClient));
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
        }

        public void Clear()
        {
            _instance = null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
