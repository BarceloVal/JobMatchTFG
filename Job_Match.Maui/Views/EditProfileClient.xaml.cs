using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class EditProfileClient : ContentPage
{
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


        // Ajusta el botón de guardar cambios
#if WINDOWS
        btnEditProfileClient.WidthRequest = 600;
#elif ANDROID
        btnEditProfileClient.WidthRequest = 350;
#endif

    }

    private EditProfileClientViewModel viewModel;

    public EditProfileClient(ProfileClientDto dto)
    {
        InitializeComponent();

        viewModel = new EditProfileClientViewModel(dto);
        BindingContext = viewModel;

        Loaded += async (_, _) => await viewModel.InitAsync();
    }

    private async void btnEditProfileClient_Clicked(object sender, EventArgs e)
    {
        // Forzar validación antes de enviar
        viewModel.ValidateFirstName();
        viewModel.ValidateLastName();
        viewModel.ValidateDocument();
        viewModel.ValidateAddress();
        viewModel.ValidatePhone1();
        viewModel.ValidatePhone2();

        if (viewModel.HasErrors)
        {
            await DisplayAlert("Errores en el formulario", "Revisa que los campos obligatorios esten rellenados.", "Aceptar");
            return;
        }

        var dto = viewModel.BuildUpdateDto();
        var success = await new Job_Match.Maui.ApiService.ApiService().UpdateClientProfileAsync(dto);

        if (success)
        {
            await DisplayAlert("Éxito", "Perfil actualizado correctamente.", "Aceptar");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el perfil.", "Aceptar");
        }
    }
}