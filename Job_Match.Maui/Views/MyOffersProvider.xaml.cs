using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class MyOffersProvider : ContentPage
{
    private readonly MyOffersProviderViewModel _viewModel;

    public MyOffersProvider()
    {
        InitializeComponent();

        // Instanciar el ViewModel
        _viewModel = new MyOffersProviderViewModel();
        BindingContext = _viewModel;

        var idUserProvider = UserSessionService.Instance.IdUserProvider ?? Guid.Empty;

        if (idUserProvider != Guid.Empty)
        {
            _ = _viewModel.LoadMyOffersAsync(idUserProvider);
            _ = _viewModel.LoadTaskStatusesAsync();
        }
        else
        {
            Console.WriteLine("[ERROR] No se encontró un ID de proveedor válido.");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var marginWinUI = new Thickness(0, 35, 10, 10);
        var marginAndroid = new Thickness(0, 0, 10, 10);
        var margin = DeviceInfo.Platform == DevicePlatform.WinUI ? marginWinUI : marginAndroid;

        imgArrowState.Margin = margin;
    }


    private async void btnQuitOffer_Clicked(object sender, EventArgs e)
    {
        if (((Button)sender).CommandParameter is SubscribedOfferDto selectedOffer)
        {
            bool confirm = await DisplayAlert("Confirmar", "¿Deseas quitarte de esta oferta?", "Sí", "Cancelar");
            if (!confirm) return;

            var idProvider = UserSessionService.Instance.IdUserProvider ?? Guid.Empty;

            if (idProvider != Guid.Empty)
            {
                await _viewModel.QuitOfferAsync(idProvider, selectedOffer.IdOffer);
            }
        }
    }

    private async void btnClean_Clicked(object sender, EventArgs e)
    {
        _viewModel.SelectedTaskStatus = null;

        var idProvider = UserSessionService.Instance.IdUserProvider ?? Guid.Empty;
        if (idProvider != Guid.Empty)
            await _viewModel.LoadMyOffersAsync(idProvider);
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
        var idProvider = UserSessionService.Instance.IdUserProvider ?? Guid.Empty;
        if (idProvider != Guid.Empty)
            await _viewModel.LoadMyOffersAsync(idProvider);
    }

}
