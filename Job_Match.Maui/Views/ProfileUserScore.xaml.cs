using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

[QueryProperty(nameof(ProviderIdString), "providerId")]
public partial class ProfileUserScore : ContentPage
{
    public string ProviderIdString { get; set; }

    private ProfileUserScoreViewModel ViewModel => BindingContext as ProfileUserScoreViewModel;

    public ProfileUserScore()
    {
        InitializeComponent();
        BindingContext = new ProfileUserScoreViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Guid.TryParse(ProviderIdString, out var parsedId))
        {
            await ViewModel.LoadUserProfileAsync(parsedId);
        }
        else
        {
            await DisplayAlert("Error", "El ID del proveedor no es válido.", "Aceptar");
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}