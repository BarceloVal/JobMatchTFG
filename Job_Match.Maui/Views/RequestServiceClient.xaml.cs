using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class RequestServiceClient : ContentPage
{
    public RequestServiceClient()
    {
        InitializeComponent();
        BindingContext = new RequestServiceClientViewModel();
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is RequestServiceClientViewModel vm)
        {
            await vm.SearchProvidersAsync();
        }
    }

    private void btnClean_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is RequestServiceClientViewModel vm)
        {
            vm.ClearFilters();
        }
    }
}