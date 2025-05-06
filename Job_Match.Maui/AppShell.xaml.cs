using Job_Match.Maui.Views;

namespace Job_Match.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar las rutas para navegación por nombre
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CreateAccount), typeof(CreateAccount));
            Routing.RegisterRoute(nameof(CreateClientAccount), typeof(CreateClientAccount));
            Routing.RegisterRoute(nameof(CreateProviderAccount), typeof(CreateProviderAccount));
            Routing.RegisterRoute(nameof(HomeClient), typeof(HomeClient));
            Routing.RegisterRoute(nameof(HomeProvider), typeof(HomeProvider));
            Routing.RegisterRoute(nameof(OfferClient), typeof(OfferClient));
            Routing.RegisterRoute(nameof(OfferProvider), typeof(OfferProvider));
            Routing.RegisterRoute(nameof(ProfileClient), typeof(ProfileClient));
            Routing.RegisterRoute(nameof(ProfileProvider), typeof(ProfileProvider));
            Routing.RegisterRoute(nameof(RequestServiceClient), typeof(RequestServiceClient));
            Routing.RegisterRoute(nameof(RequestServiceProvider), typeof(RequestServiceProvider));
            Routing.RegisterRoute(nameof(HelpProvider), typeof(HelpProvider));
            Routing.RegisterRoute(nameof(HelpClient), typeof(HelpClient));
            Routing.RegisterRoute(nameof(ChangeProfileToProvider), typeof(ChangeProfileToProvider));
            Routing.RegisterRoute(nameof(ChangeProfileToClient), typeof(ChangeProfileToClient));
        }
    }
}
