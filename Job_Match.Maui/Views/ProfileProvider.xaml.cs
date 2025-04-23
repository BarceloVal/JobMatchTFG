using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ProfileProvider : ContentPage
{
    public ProfileProvider()
    {
        InitializeComponent();
        BindingContext = new ProfileProviderViewModel();
    }

    private void btnSwitchUser_Clicked(object sender, EventArgs e)
    {

    }
}