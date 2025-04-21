using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class CreateClientAccount : ContentPage
{
    public CreateClientAccount()
    {
        InitializeComponent();
        BindingContext = new CreateClientAccountViewModel();
    }

    // Variable para que la primera contraseña pueda ser visible o no
    private bool isPasswordVisible1 = false;

    /// <summary>
    /// Alterna visibilidad de la primera contraseña.
    /// </summary>
    private void btnPasswordVisibility_Clicked(object sender, EventArgs e)
    {
        isPasswordVisible1 = !isPasswordVisible1;
        ePasswordClient1.IsPassword = !isPasswordVisible1;

        var button = sender as ImageButton;

        // Cambiar el ícono
        button.Source = isPasswordVisible1 ? "passwordon.png" : "passwordoff.png";

    }

    // Variable para que la repetición de la contraseña pueda ser visible o no
    private bool isPasswordVisible2 = false;

    /// <summary>
    /// Alterna visibilidad de la repetición de contraseña.
    /// </summary>
    private void btnPasswordVisibility2_Clicked(object sender, EventArgs e)
    {
        isPasswordVisible2 = !isPasswordVisible2;
        ePasswordClient2.IsPassword = !isPasswordVisible2;

        var button = sender as ImageButton;

        // Cambiar el ícono
        button.Source = isPasswordVisible2 ? "passwordon.png" : "passwordoff.png";
    }

    /// <summary>
    /// Maneja la creación de la cuenta del cliente.
    /// </summary>
    private async void btnCreateClient_Clicked(object sender, EventArgs e)
    {
        var vm = BindingContext as CreateClientAccountViewModel;

        bool result = await vm.CreateClientAsync();

        if (result)
        {
            await DisplayAlert("Éxito", "Cuenta creada correctamente", "Aceptar");
        }
        else
        {
            await DisplayAlert("Error", "Validación fallida en uno o más campos. Revisá la consola para más detalles.", "OK");
        }
    }


}