using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Newtonsoft.Json;

namespace Job_Match.Maui.Views;

[QueryProperty(nameof(JsonData), "data")]
public partial class OfferProvidersSubscription : ContentPage
{
    public string JsonData
    {
        set
        {
            var offer = JsonConvert.DeserializeObject<TaskRequestWithProvidersDto>(Uri.UnescapeDataString(value));
            OfferTitle = offer.Title;
            Providers = offer.Providers ?? new();
            BindingContext = this;
        }
    }

    public string OfferTitle { get; set; }
    public List<ProviderTaskRequestDto> Providers { get; set; } = new();

    public OfferProvidersSubscription()
    {
        InitializeComponent();
    }

    private async void btnBackToOfferClient_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void btnContact_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine($"Sender: {sender?.GetType()}");
        Console.WriteLine($"BindingContext: {((Button)sender).BindingContext}");

        if (sender is Button btn && btn.BindingContext is ProviderTaskRequestDto provider)
        {
            var idClientUser = UserSessionService.Instance.Id; // ID del cliente logueado
            var idProviderUser = provider.IdUser;              // ID del proveedor

            var dto = new StartChatDto
            {
                IdUserClient = idClientUser,
                IdUserProvider = idProviderUser
            };

            var api = new Job_Match.Maui.ApiService.ApiService();
            var result = await api.PostAsync("api/task-contacts/contact-provider", dto);

            if (result)
            {
                await Shell.Current.GoToAsync($"{nameof(ChatConversationPage)}?IdClientUser={idClientUser}&IdProviderUser={idProviderUser}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo iniciar el chat.", "OK");
            }
        }
    }
}