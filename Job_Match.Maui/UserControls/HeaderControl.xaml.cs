using Job_Match.Maui.ApiService;

namespace Job_Match.Maui.UserControls;

public partial class HeaderControl : ContentPage
{
    public HeaderControl()
    {
        InitializeComponent();

        var tipo = UserSessionService.Instance.IsClient
            ? "Cliente"
            : UserSessionService.Instance.IsProvider
                ? "Proveedor"
                : "Desconocido";

        lblUserType.Text = tipo;
    }

    private void OnMenuClicked_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}