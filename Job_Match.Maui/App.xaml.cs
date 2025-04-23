using Job_Match.Data.Models.Dtos;

namespace Job_Match.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // LoginPage por defecto
            MainPage = new AppShell();
        }
        public static object CurrentUser { get; set; }

    }
}
