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
        }
        else
        {
            Console.WriteLine("[ERROR] No se encontró un ID de proveedor válido.");
        }
    }

    private async void btnBackToMenuProvider_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
