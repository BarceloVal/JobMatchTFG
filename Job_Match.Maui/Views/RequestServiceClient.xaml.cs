using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class RequestServiceClient : ContentPage
{
    public RequestServiceClient()
    {
        InitializeComponent();
        BindingContext = new RequestServiceClientViewModel();
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is RequestServiceClientViewModel vm)
        {
            await vm.SearchProvidersAsync();
        }
    }

    private void btnClean_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is RequestServiceClientViewModel vm)
        {
            vm.ClearFilters();
        }
    }

    private async void btnContact_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is ProviderWithServicesDto provider)
        {
            var idClientUser = UserSessionService.Instance.Id; // m_user.id del cliente
            var idProviderUser = provider.IdUser;              // m_user.id del proveedor

            var dto = new StartChatDto
            {
                IdUserClient = idClientUser,
                IdUserProvider = idProviderUser
            };

            Console.WriteLine($"Contactando con proveedor: IdClient={idClientUser}, IdProvider={idProviderUser}");

            var api = new Job_Match.Maui.ApiService.ApiService();
            var result = await api.PostAsync("api/task-contacts/contact-provider", dto);

            if (result)
            {
                // Navegar a la pantalla de conversación (opcionalmente podés pasar el nombre del proveedor también)
                await Shell.Current.GoToAsync($"{nameof(ChatConversationPage)}?IdClientUser={idClientUser}&IdProviderUser={idProviderUser}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo iniciar el chat.", "OK");
            }
        }
    }

}