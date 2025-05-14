using Job_Match.Data.Models.Dtos;
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
}