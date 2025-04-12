using Job_Match.Maui.Views;
using Job_Match.Maui.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Job_Match.Maui.Views;
public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();
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

            // Controlaría vacios

            var api = new Job_Match.Maui.ApiService.ApiService();
            var login_response = await api.LoginAsync(e_mail, e_password);

            if (login_response != null)
            {
                App.CurrentUser = login_response.user;
                var user = login_response.user;

                if (user.is_client)
                {
                    Application.Current.MainPage = new MainShellClient();
                    await Shell.Current.GoToAsync("//HomeClient");
                }
                else if (user.is_provider)
                {
                    Application.Current.MainPage = new MainShellProvider();
                    await Shell.Current.GoToAsync("//HomeProvider");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo determinar si el usuario es cliente o proveedor.", "Aceptar");
                }
            }
            else if (e_mail.IsNullOrEmpty() || e_password.IsNullOrEmpty())
            {
                await DisplayAlert("Error", "Hay campos vacíos. Asegurate de rellenar todos los campos para iniciar sesión", "Aceptar");
            }
            else
            {
                await DisplayAlert("Error", "Hubo un error en el reconocimiento del usuario. Revisa si el usuario o la contraseña son correctos.", "Aceptar");
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
}
