using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views
{
    [QueryProperty(nameof(ProviderIdString), "providerId")]
    public partial class ScoreUser : ContentPage
    {
        public string ProviderIdString { get; set; }

        private int _currentRating = 0;

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

        private void btnStar1_Clicked(object sender, EventArgs e) => SetRating(1);
        private void btnStar2_Clicked(object sender, EventArgs e) => SetRating(2);
        private void btnStar3_Clicked(object sender, EventArgs e) => SetRating(3);
        private void btnStar4_Clicked(object sender, EventArgs e) => SetRating(4);
        private void btnStar5_Clicked(object sender, EventArgs e) => SetRating(5);

        private void SetRating(int rating)
        {
            _currentRating = rating;

            btnStar1.Source = rating >= 1 ? "starfill.png" : "starempty.png";
            btnStar2.Source = rating >= 2 ? "starfill.png" : "starempty.png";
            btnStar3.Source = rating >= 3 ? "starfill.png" : "starempty.png";
            btnStar4.Source = rating >= 4 ? "starfill.png" : "starempty.png";
            btnStar5.Source = rating >= 5 ? "starfill.png" : "starempty.png";
        }
    }
}