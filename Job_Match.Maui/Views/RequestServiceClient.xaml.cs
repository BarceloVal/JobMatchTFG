using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class RequestServiceClient : ContentPage
{
    public RequestServiceClient()
    {
        InitializeComponent();
        BindingContext = new RequestServiceClientViewModel();
    }
}