namespace Job_Match.Maui.Views;

public partial class CreateAccount : ContentPage
{
    public CreateAccount()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Redirige a la creación de cuenta de proveedor.
    /// </summary>
    private async void btnProvider_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateProviderAccount));
    }

    /// <summary>
    /// Redirige a la creación de cuenta de cliente.
    /// </summary>
    private async void btnClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateClientAccount));
    }
}