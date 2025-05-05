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

    private void btnDisableAccount_Clicked(object sender, EventArgs e)
    {

    }
}