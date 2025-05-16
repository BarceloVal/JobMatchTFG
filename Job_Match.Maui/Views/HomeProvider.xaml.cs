namespace Job_Match.Maui.Views;

public partial class HomeProvider : ContentPage
{
    public HomeProvider()
    {
        InitializeComponent();
    }

    private async void btnrequest_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RequestServiceProvider));
    }

    private async void btnoffers_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OfferProvider));
    }

    private async void btnMyOffers_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyOffersProvider));
    }
}