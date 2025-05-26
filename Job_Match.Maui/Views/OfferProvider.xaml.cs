using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class OfferProvider : ContentPage
{
    public OfferProvider()
    {
        InitializeComponent();
        BindingContext = new OfferProviderViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var marginWinUI = new Thickness(0, 35, 10, 10);
        var marginAndroid = new Thickness(0, 0, 10, 10);
        var margin = DeviceInfo.Platform == DevicePlatform.WinUI ? marginWinUI : marginAndroid;

        imgArrowPostalCode.Margin = margin;
        imgArrowServiceType.Margin = margin;
        imgArrowService.Margin = margin;
    }

    private async void btnClean_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is OfferProviderViewModel vm)
        {
            vm.ClearFilters();
        }
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is OfferProviderViewModel vm)
        {
            await vm.SearchOpenOffersAsync();
        }
    }

    private async void btnSubscribeOffer_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var offer = button?.CommandParameter as TaskRequestWithOfferDto;

        if (offer == null)
            return;

        try
        {
            var apiService = new Job_Match.Maui.ApiService.ApiService();

            var idUser = UserSessionService.Instance.Id;
            var idProvider = await apiService.GetProviderIdFromUserIdAsync(idUser);

            var dto = new OfferSubscriptionDto
            {
                IdTaskRequest = offer.Id,
                IdUserProvider = idProvider
            };

            var response = await apiService.SubscribeToOfferWithResponseAsync(dto);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                await DisplayAlert("Aviso", "Ya estás inscrito a esta oferta", "Aceptar");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Error al inscribirse", "Aceptar");
                return;
            }

            await DisplayAlert("Éxito", "Te has inscrito a la oferta", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo inscribir: {ex.Message}", "Aceptar");
        }
    }
}