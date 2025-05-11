using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ViewModels;
using Job_Match.Maui.ApiService;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;

namespace Job_Match.Maui.Views
{
    public partial class ProfileClient : ContentPage
    {
        private readonly ApiServiceClass _apiService = new();

        public ProfileClient()
        {
            InitializeComponent();

            BindingContext = new ProfileClientViewModel(App.CurrentUser as ProfileClientDto);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var dto = await _apiService.GetClientByMailAsync(UserSessionService.Instance.Mail);

            App.CurrentUser = dto;

            BindingContext = new ProfileClientViewModel(dto);
        }

        private async void btnSwitchUser_Clicked(object sender, EventArgs e)
        {
            var confirmed = await DisplayAlert(
            "Cambio de perfil",
            "¿Seguro que deseas cambiar tu perfil a proveedor? Si es la primera vez que lo haces necesitamos que completes algunos datos adicionales.",
            "Aceptar", "Cancelar");

            if (!confirmed) return;

            var session = UserSessionService.Instance;
            var exists = await _apiService.ProviderExistsAsync(session.Id);

            if (exists)
            {
                var dto = new ChangeClientToProviderDto
                {
                    IdUser = session.Id,
                    Password = session.Password
                };

                var success = await _apiService.ChangeClientToProviderAsync(dto);

                if (success)
                {
                    await DisplayAlert("Éxito", "Perfil cambiado a proveedor.", "Aceptar");
                    await Shell.Current.GoToAsync("LoginPage");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo cambiar el perfil. Intenta de nuevo.", "Aceptar");
                }
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(ChangeProfileToProvider));
            }
        }

        private async void btnDisableAccount_Clicked(object sender, EventArgs e)
        {
            var confirmed = await DisplayAlert(
                "Desactivar cuenta",
                "¿Estás seguro de que deseas desactivar tu cuenta? No podrás usarla hasta reactivarla.",
                "Sí", "Cancelar");

            if (!confirmed) return;

            _apiService.SetAuthToken(UserSessionService.Instance.Token);

            var session = UserSessionService.Instance;
            var success = await _apiService.DisableAccountAsync(UserSessionService.Instance.Id);

            if (success)
            {
                await DisplayAlert("Cuenta desactivada", "Tu cuenta ha sido desactivada correctamente.", "Aceptar");
                await Shell.Current.GoToAsync("LoginPage");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo desactivar la cuenta. Intenta de nuevo.", "Aceptar");
            }
        }

        private async void btnEditProfileClient_Clicked(object sender, EventArgs e)
        {
            var dto = await _apiService.GetClientByMailAsync(UserSessionService.Instance.Mail);
            await Navigation.PushAsync(new EditProfileClient(dto));
        }
    }
}
