using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;

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
    }

    private async void btnCreateOffer_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreateOffer");
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnEdit_Clicked(object sender, EventArgs e)
    {

    }
}