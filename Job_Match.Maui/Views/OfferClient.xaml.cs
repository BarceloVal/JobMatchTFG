namespace Job_Match.Maui.Views;

public partial class OfferClient : ContentPage
{
    public OfferClient()
    {
        InitializeComponent();
    }

    private async void btnCreateOffer_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CreateOffer");
    }
}