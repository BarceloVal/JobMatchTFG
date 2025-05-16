using Job_Match.Data.Models.Dtos;
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

        private void ChatConversationPage_BindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is ChatConversationViewModel vm)
            {
                vm.Messages.CollectionChanged += (s, ev) =>
                {
                    // Esperar un poco para asegurar que el nuevo mensaje se renderice
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (MessagesList.ItemsSource is IList<ChatMessageDto> messages && messages.Count > 0)
                        {
                            MessagesList.ScrollTo(messages.Last(), position: ScrollToPosition.End, animate: true);
                        }
                    });
                };
            }
        }
    }
}
