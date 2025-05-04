using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ProfileClient : ContentPage
{
    public ProfileClient()
    {
        InitializeComponent();

        // Asigno el usuario logueado con contexto de datos
        BindingContext = new ProfileClientViewModel(App.CurrentUser as ProfileClientDto);
    }

    private async void btnSwitchUser_Clicked(object sender, EventArgs e)
    {
        var confirmed = await DisplayAlert(
            "Cambio de perfil",
            "¿Seguro que deseas cambiar tu perfil a proveedor? Si es la primera vez que lo haces necesitamos que completes algunos datos adicionales.",
            "Aceptar", "Cancelar");

        if (confirmed)
        {
            await Shell.Current.GoToAsync(nameof(ChangeProfileToProvider));
        }

    }

    private void btnDisableAccount_Clicked(object sender, EventArgs e)
    {

    }
}