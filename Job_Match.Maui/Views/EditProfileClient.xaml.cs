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
        var dto = viewModel.BuildUpdateDto();
        var success = await new Job_Match.Maui.ApiService.ApiService().UpdateClientProfileAsync(dto);

        if (success)
        {
            await DisplayAlert("Éxito", "Perfil actualizado correctamente.", "Aceptar");
            await Shell.Current.GoToAsync(".."); // Vuelve atrás
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el perfil.", "Aceptar");
        }
    }
}