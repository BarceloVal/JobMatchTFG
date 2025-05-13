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

            var idUser = UserSessionService.Instance.Id; // este es el id de m_user
            var idProvider = await apiService.GetProviderIdFromUserIdAsync(idUser);

            var dto = new OfferSubscriptionDto
            {
                IdTaskRequest = offer.Id,
                IdUserProvider = idProvider
            };

            await apiService.SubscribeToOfferAsync(dto);
            await DisplayAlert("Éxito", "Te has inscrito a la oferta", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo inscribir: {ex.Message}", "OK");
        }
    }
}