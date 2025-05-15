using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class EditProfileProvider : ContentPage
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

    private EditProfileProviderViewModel viewModel;

    public EditProfileProvider(ProfileProviderDto dto)
    {
        InitializeComponent();

        viewModel = new EditProfileProviderViewModel(dto);
        BindingContext = viewModel;

        Loaded += async (_, _) => await viewModel.InitAsync();
    }

    private async void btnEditProfile_Clicked(object sender, EventArgs e)
    {
        if (!await viewModel.ValidateAllAsync())
        {
            await DisplayAlert("Errores en el formulario", "Revisa que los campos obligatorios esten rellenados.", "Aceptar");
            return;
        }

        var dto = viewModel.BuildUpdateDto();
        var apiService = new Job_Match.Maui.ApiService.ApiService();

        var success = await apiService.UpdateProviderProfileAsync(dto);

        if (success)
        {
            await DisplayAlert("Éxito", "Perfil actualizado correctamente.", "Aceptar");

            var profile = await apiService.GetProviderByMailAsync(((ProfileProviderDto)App.CurrentUser).Mail);
            if (profile != null)
            {
                await Shell.Current.GoToAsync("ProfileProvider");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo recargar el perfil actualizado.", "Aceptar");
            }

        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el perfil.", "Aceptar");
        }
    }

    private void radBtnServices_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        var selectedService = (sender as RadioButton)?.BindingContext as EditProfileProviderViewModel.ServiceSelectable;
        if (selectedService == null)
            return;

        var vm = BindingContext as EditProfileProviderViewModel;
        if (vm == null)
            return;

        foreach (var service in vm.FilteredServices)
        {
            service.IsSelected = service == selectedService;
        }

        vm.SelectedService = selectedService;

        Console.WriteLine($"[DEBUG] Nuevo servicio seleccionado: {selectedService.name} - {selectedService.id}");
    }

    private void eDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue.Length;
        lblCharCount.Text = $"{count}/1000";
    }
}