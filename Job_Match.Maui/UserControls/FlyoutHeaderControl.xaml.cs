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
        if (App.CurrentUser != null)
            lblHeader.Text = $"Sesión de {App.CurrentUser.first_name}";
        else
            lblHeader.Text = "Sesión activa";
    }
}