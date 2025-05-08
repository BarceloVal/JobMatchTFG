namespace Job_Match.Maui.Views;

public partial class CreateAccount : ContentPage
{
    public CreateAccount()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        string htmlAndroid = @"
        <html>
            <head><meta name='viewport' content='width=device-width, initial-scale=1.0'></head>
            <body style='text-align:justify; font-size:17px; color:black; margin:0; padding:0;'>
                <p>Regístrate como cliente si buscas contratar servicios, o como proveedor si deseas ofrecer tus servicios profesionales y experiencia. </p>
                <p>Más adelante, podrás cambiar entre cliente y proveedor desde las configuraciones de tu cuenta.</p>
            </body>
        </html>";

        string htmlWinUI = @"
        <html>
            <head><meta name='viewport' content='width=device-width, initial-scale=1.0'></head>
            <body style='text-align:justify; font-size:17px; color:black; margin:0; padding:0;'>
                <p>Regístrate como cliente si buscas contratar servicios especializados de forma fácil y segura, o como proveedor si deseas ofrecer tus habilidades profesionales, experiencia y conocimientos a una comunidad de usuarios que valora la calidad.</p>
                <p>Más adelante, si lo necesitas, podrás cambiar entre cliente y proveedor desde las configuraciones de tu cuenta, sin perder tu información ni tus interacciones previas.</p>
                <p>Únete a JobMatch y forma parte de una red que conecta talento con oportunidades.</p>
            </body>
        </html>";

        DescriptionWebView.Source = new HtmlWebViewSource
        {
            Html = DeviceInfo.Platform == DevicePlatform.WinUI ? htmlWinUI : htmlAndroid
        };
    }

    /// <summary>
    /// Redirige a la creación de cuenta de proveedor.
    /// </summary>
    private async void btnProvider_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateProviderAccount));
    }

    /// <summary>
    /// Redirige a la creación de cuenta de cliente.
    /// </summary>
    private async void btnClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateClientAccount));
    }
}