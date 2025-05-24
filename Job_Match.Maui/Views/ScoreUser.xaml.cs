using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

[QueryProperty(nameof(ProviderIdString), "providerId")]
public partial class ScoreUser : ContentPage
{
    public string ProviderIdString { get; set; }

    private ScoreUserViewModel ViewModel => BindingContext as ScoreUserViewModel;

    public ScoreUser()
    {
        InitializeComponent();
        BindingContext = new ScoreUserViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Guid.TryParse(ProviderIdString, out var parsedId))
        {
            await ViewModel.LoadUserScoreAsync(parsedId);
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