using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;
using Newtonsoft.Json;

namespace Job_Match.Maui.Views;

public partial class OfferClient : ContentPage
{
    private OfferClientViewModel _viewModel;
    public OfferClient()
    {
        InitializeComponent();
        _viewModel = new OfferClientViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var clientId = UserSessionService.Instance.IdUserClient;

        if (clientId != null)
        {
            await _viewModel.LoadActiveOffersAsync(clientId.Value);
        }
        else
        {
            Console.WriteLine("No se encontró el ID del cliente en la sesión.");
        }

        if (clientId != null)
        {
            await _viewModel.LoadActiveOffersAsync(clientId.Value);
        }
    }

    private async void btnCreateOffer_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreateOffer");
    }

    private async void btnEdit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedOffer = button?.CommandParameter as TaskRequestWithProvidersDto;

        if (selectedOffer != null)
        {
            var json = JsonConvert.SerializeObject(selectedOffer);
            await Shell.Current.GoToAsync($"EditOffer?data={Uri.EscapeDataString(json)}");
        }
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is TaskRequestWithProvidersDto offer)
        {
            bool confirm = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar esta oferta?", "Sí", "No");
            if (!confirm) return;

            var success = await new Job_Match.Maui.ApiService.ApiService().DeactivateOfferAsync(offer.Id);
            if (success)
            {
                await DisplayAlert("Eliminada", "La oferta fue eliminada correctamente.", "Aceptar");

                var clientId = UserSessionService.Instance.IdUserClient;
                if (clientId.HasValue)
                {
                    await _viewModel.LoadActiveOffersAsync(clientId.Value);
                }
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar la oferta.", "Aceptar");
            }
        }
    }
}