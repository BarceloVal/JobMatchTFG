using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ChangeProfileToClient : ContentPage
{
    private readonly ChangeProfileToClientViewModel _viewModel;
    public ChangeProfileToClient()
    {
        InitializeComponent();
        _viewModel = new ChangeProfileToClientViewModel();
        BindingContext = _viewModel;
    }

    private async void btnChangeClient_Clicked(object sender, EventArgs e)
    {
        var result = await _viewModel.ChangeToClientAsync();

        if (result)
        {
            await Shell.Current.DisplayAlert("Éxito", "Tu perfil se ha actualizado como cliente.", "Aceptar");
            await Shell.Current.GoToAsync("LoginPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Corrige los errores e intenta de nuevo.", "Aceptar");
        }
    }
}