using Job_Match.MAUI.ViewModels;

namespace Job_Match.Maui.Views
{



    [QueryProperty(nameof(IdClientUser), "IdClientUser")]
    [QueryProperty(nameof(IdProviderUser), "IdProviderUser")]
    public partial class ChatConversationPage : ContentPage
    {
        public Guid IdClientUser { get; set; }
        public Guid IdProviderUser { get; set; }

        public ChatConversationPage()
        {
            InitializeComponent();
            BindingContext = new ChatConversationViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ChatConversationViewModel vm)
                await vm.LoadMessagesAsync(IdClientUser, IdProviderUser);
        }
    }
}
