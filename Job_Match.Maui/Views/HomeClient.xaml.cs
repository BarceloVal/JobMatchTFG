namespace Job_Match.Maui.Views;

public partial class HomeClient : ContentPage
{
    public HomeClient()
    {
        InitializeComponent();
    }

    private async void btnrequest_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RequestServiceClient));
    }

    private async void btnoffers_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OfferClient));
    }
}