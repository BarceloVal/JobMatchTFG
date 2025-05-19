using Job_Match.Data.Models.Dtos;
using Job_Match.MAUI.ViewModels;

namespace Job_Match.Maui.Views
{
    [QueryProperty(nameof(IdClientUser), "IdClientUser")]
    [QueryProperty(nameof(IdProviderUser), "IdProviderUser")]
    public partial class ChatConversationPage : ContentPage
    {
        private Guid _idClientUser;
        private Guid _idProviderUser;

        public string IdClientUser
        {
            set
            {
                if (Guid.TryParse(value, out var parsed))
                {
                    _idClientUser = parsed;
                    _ = TryInitialize();
                }
            }
        }

        public string IdProviderUser
        {
            set
            {
                if (Guid.TryParse(value, out var parsed))
                {
                    _idProviderUser = parsed;
                    _ = TryInitialize();
                }
            }
        }

        private async Task TryInitialize()
        {
            if (_idClientUser != Guid.Empty && _idProviderUser != Guid.Empty && ViewModel != null)
            {
                await ViewModel.LoadMessagesAsync(_idClientUser, _idProviderUser);
            }
        }

        private ChatConversationViewModel ViewModel => BindingContext as ChatConversationViewModel;

        public ChatConversationPage()
        {
            InitializeComponent();
            BindingContext = new ChatConversationViewModel();
            BindingContextChanged += ChatConversationPage_BindingContextChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
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

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
