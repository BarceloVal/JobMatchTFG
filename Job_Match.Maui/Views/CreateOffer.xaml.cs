using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class CreateOffer : ContentPage
{
    public CreateOffer()
    {
        InitializeComponent();
        BindingContext = new CreateOfferViewModel();
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

    private async void btnCreateOffer_Clicked(object sender, EventArgs e)
    {
        var vm = BindingContext as CreateOfferViewModel;

        bool result = await vm.CreateOfferAsync();

        if (result)
        {
            await DisplayAlert("Éxito", "Oferta creada correctamente", "Aceptar");
            await Shell.Current.GoToAsync("OfferClient");
        }
        else
        {
            await DisplayAlert("Error", "Revisa que los campos obligatorios esten rellenos.", "Aceptar");
        }
    }

    private void radBtnServices_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        var selectedService = (sender as RadioButton)?.BindingContext as CreateProviderAccountViewModel.ServiceSelectable;
        if (selectedService == null)
            return;

        var vm = BindingContext as CreateProviderAccountViewModel;
        if (vm == null)
            return;

        foreach (var service in vm.FilteredServices)
        {
            service.IsSelected = service == selectedService;
        }
    }

    private void eDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue.Length;
        lblCharCount.Text = $"{count}/1000";
    }
}