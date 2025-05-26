using Job_Match.Maui.Views;
using Job_Match.Maui.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;

namespace Job_Match.Maui.Views;
public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();

        LoadSavedCredentials();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ajusta la decoración del Login
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            PathAndroid.IsVisible = true;
            PathWin.IsVisible = false;
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            PathAndroid.IsVisible = false;
            PathWin.IsVisible = true;
        }

    }

    /// <summary>
    /// Maneja el evento de clic en el botón de iniciar sesión.
    /// Realiza la autenticación y redirige a la vista correspondiente.
    /// </summary>
    private async void btnSesion_Clicked(object sender, EventArgs e)
    {
        try
        {
            var e_mail = eMail.Text;
            var e_password = ePassword.Text;

            // Guardar o eliminar datos recordados
            if (cbRememberPassword.IsChecked)
            {
                Preferences.Set("SavedPassword", e_password);
                Preferences.Set("RememberPassword", true);
            }
            else
            {
                Preferences.Remove("SavedPassword");
                Preferences.Set("RememberPassword", false);
            }

            var api = new Job_Match.Maui.ApiService.ApiService();
            var login_response = await api.LoginAsync(e_mail, e_password);

            if (login_response == null)
            {
                await DisplayAlert("Error", "Usuario y contraseña incorrecto. Vuelve a intentarlo.", "Aceptar");
                return;
            }

            // Obtener perfil según tipo de usuario
            object profile = null;

            if (login_response.user.is_client)
            {
                var clientProfile = await api.GetClientByMailAsync(login_response.user.mail);
                if (clientProfile == null)
                {
                    await DisplayAlert("Error", "No se pudo obtener el perfil del cliente.", "Aceptar");
                    return;
                }

                App.CurrentUser = clientProfile;

                Application.Current.MainPage = new MainShellClient();
                await Shell.Current.GoToAsync("HomeClient");
            }
            else if (login_response.user.is_provider)
            {
                var providerProfile = await api.GetProviderByMailAsync(login_response.user.mail);
                if (providerProfile == null)
                {
                    await DisplayAlert("Error", "No se pudo obtener el perfil del proveedor.", "Aceptar");
                    return;
                }

                App.CurrentUser = providerProfile;

                Application.Current.MainPage = new MainShellProvider();
                await Shell.Current.GoToAsync("HomeProvider");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo determinar si el usuario es cliente o proveedor.", "Aceptar");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }


    }

    /// <summary>
    /// Navega a la pantalla de creación de cuenta.
    /// </summary>
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateAccount));
    }

    /// <summary>
    /// Alterna la visibilidad de la contraseña.
    /// </summary>
    private bool isPasswordVisible = false;
    private void btnPasswordVisibility_Clicked(object sender, EventArgs e)
    {
        isPasswordVisible = !isPasswordVisible;
        ePassword.IsPassword = !isPasswordVisible;

        var button = sender as ImageButton;

        // Cambiar el ícono
        button.Source = isPasswordVisible ? "passwordon.png" : "passwordoff.png";
    }

    private void LoadSavedCredentials()
    {
        bool remember = Preferences.Get("RememberPassword", false);
        cbRememberPassword.IsChecked = remember;

        if (remember)
        {
            ePassword.Text = Preferences.Get("SavedPassword", string.Empty);
        }
    }
}
