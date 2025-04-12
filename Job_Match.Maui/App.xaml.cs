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
        public static Models._UserDto CurrentUser { get; set; }
    }
}
