using Job_Match.Data.Models.Dtos;

namespace Job_Match.Maui.UserControls;

public partial class FlyoutHeaderControl : ContentView
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();
        Update();
    }

    public void Update()
    {
        if (App.CurrentUser is ProfileClientDto client)
        {
            lblHeader.Text = $"Sesión de {client.FirstName}";
        }
        else if (App.CurrentUser is ProfileProviderDto provider)
        {
            lblHeader.Text = $"Sesión de {provider.FirstName}";
        }
        else
        {
            lblHeader.Text = "Sesión activa";
        }
    }
}