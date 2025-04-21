using Job_Match.Maui.ViewModels;
using System.Globalization;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;

namespace Job_Match.Maui.Views;

public partial class CreateProviderAccount : ContentPage
{
    public CreateProviderAccount()
    {
        InitializeComponent();
        BindingContext = new CreateProviderAccountViewModel();
    }

    // Variable para que la primera contraseña pueda ser visible o no
    private bool isPasswordVisible1 = false;

    /// <summary>
    /// Alterna visibilidad de la primera contraseña.
    /// </summary>
    private void btnPasswordVisibility1_Clicked(object sender, EventArgs e)
    {
        isPasswordVisible1 = !isPasswordVisible1;
        ePasswordProvider1.IsPassword = !isPasswordVisible1;

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
        ePasswordProvider2.IsPassword = !isPasswordVisible2;

        var button = sender as ImageButton;

        // Cambiar el ícono
        button.Source = isPasswordVisible2 ? "passwordon.png" : "passwordoff.png";
    }

    /// <summary>
    /// Actualiza el contador de caracteres de la descripción.
    /// </summary>
    private void eDescription_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue.Length;
        lblCharCount.Text = $"{count}/1000";
    }

    /// <summary>
    /// Maneja la creación de la cuenta de proveedor.
    /// </summary>
    private async void btnCreateProvider_Clicked(object sender, EventArgs e)
    {
        var vm = BindingContext as CreateProviderAccountViewModel;

        bool result = await vm.CreateProviderAsync();

        if (result)
        {
            await DisplayAlert("Éxito", "Proveedor creado correctamente", "Aceptar");
        }
        else
        {
            await DisplayAlert("Error", "Revisá los campos del formulario o la conexión con el servidor", "OK");
        }
    }
}