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
}