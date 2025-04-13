namespace Job_Match.Maui;

public partial class MainShellProvider : Shell
{
    public MainShellProvider()
    {
        InitializeComponent();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (this.FlyoutHeader is Job_Match.Maui.UserControls.FlyoutHeaderControl header)
            {
                header.Update();
            }
        });
    }

    private void btnLogOut_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}