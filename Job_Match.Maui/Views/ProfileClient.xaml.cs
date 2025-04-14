using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ProfileClient : ContentPage
{
    public ProfileClient()
    {
        InitializeComponent();

        // Asigno el usuario logueado con contexto de datos
        BindingContext = new ProfileClientViewModel(App.CurrentUser);
    }

    private void btnSwitchUser_Clicked(object sender, EventArgs e)
    {

    }
}