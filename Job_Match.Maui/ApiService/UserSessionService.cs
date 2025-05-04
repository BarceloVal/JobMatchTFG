using Job_Match.Maui.Models;

namespace Job_Match.Maui.ApiService
{
    public class UserSessionService
    {
        private static UserSessionService _instance;
        public static UserSessionService Instance => _instance ??= new UserSessionService();

        public Guid Id { get; private set; }
        public string Mail { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsClient { get; private set; }
        public bool IsProvider { get; private set; }

        //public string FullName => $"{FirstName} {LastName}";
        public string Token { get; private set; }
        public string? Password { get; internal set; }

        private UserSessionService() { }

        public void Load(LoginResponse loginResponse, string plainPassword)
        {
            var user = loginResponse.user;

            Id = user.id;
            Mail = user.mail;
            Password = plainPassword;
            FirstName = user.first_name;
            LastName = user.last_name;
            IsClient = user.is_client;
            IsProvider = user.is_provider;
            Token = loginResponse.token;
        }

        public void Clear()
        {
            _instance = null;
        }
    }
}