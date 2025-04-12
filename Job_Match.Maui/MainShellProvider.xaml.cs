namespace Job_Match.Maui;

public partial class MainShellProvider : Shell
{
    public MainShellProvider()
    {
        InitializeComponent();
    }

    private void btnLogOut_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}