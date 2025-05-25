using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;
using System.Xml.Linq;

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

        private async void btnScore_Clicked(object sender, EventArgs e)
        {
            if (_currentRating == 0)
            {
                await DisplayAlert("Atención", "Debes seleccionar una valoración entre 1 y 5 estrellas.", "Aceptar");
                return;
            }

            var comment = txtComment.Text?.Trim();
            var dto = new CreateTaskScoreDto
            {
                IdUserReviewer = UserSessionService.Instance.Id,
                IdUserReviewed = ViewModel.IdUserReviewed,
                Rating = _currentRating,
                Comment = comment
            };

            var success = await new Job_Match.Maui.ApiService.ApiService().CreateTaskScoreAsync(dto);
            if (success)
            {
                await DisplayAlert("Éxito", "La valoración fue enviada correctamente.", "Aceptar");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo enviar la valoración. Intenta nuevamente.", "Aceptar");
            }
        }

        private void txtComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            int count = e.NewTextValue.Length;
            lblCharCount.Text = $"{count}/1000";
        }
    }
}