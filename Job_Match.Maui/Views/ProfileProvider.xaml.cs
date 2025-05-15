using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;

namespace Job_Match.Maui.Views;

public partial class ProfileProvider : ContentPage
{
    private readonly ApiServiceClass _apiService = new();
    public ProfileProvider()
    {
        InitializeComponent();
        BindingContext = new ProfileProviderViewModel(App.CurrentUser as ProfileProviderDto);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var updatedDto = await _apiService.GetProviderByMailAsync(UserSessionService.Instance.Mail);
        App.CurrentUser = updatedDto;
        BindingContext = new ProfileProviderViewModel(updatedDto);
    }

    private async void btnSwitchUser_Clicked(object sender, EventArgs e)
    {
        var confirmed = await DisplayAlert(
        "Cambio de perfil",
            "¿Seguro que deseas cambiar tu perfil a cliente? Si es la primera vez que lo haces necesitamos que completes algunos datos adicionales.",
            "Aceptar", "Cancelar");

        if (!confirmed) return;

        var session = UserSessionService.Instance;
        var exists = await _apiService.ClientExistsAsync(session.Id);

        if (exists)
        {
            var dto = new ChangeProviderToClientDto
            {
                IdUser = session.Id,
                Password = session.Password
            };

            var success = await _apiService.ChangeProviderToClientAsync(dto);

            if (success)
            {
                await DisplayAlert("Éxito", "Perfil cambiado a cliente.", "Aceptar");
                await Shell.Current.GoToAsync("LoginPage");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo cambiar el perfil. Intenta de nuevo.", "Aceptar");
            }
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(ChangeProfileToClient));
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

    private async void btnEditProfile_Clicked(object sender, EventArgs e)
    {
        var dto = await _apiService.GetProviderByMailAsync(UserSessionService.Instance.Mail);
        await Navigation.PushAsync(new EditProfileProvider(dto));
    }
}