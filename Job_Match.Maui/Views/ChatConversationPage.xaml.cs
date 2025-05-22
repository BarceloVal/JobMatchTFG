using Job_Match.Data.Models.Dtos;
using Job_Match.MAUI.ViewModels;

namespace Job_Match.Maui.Views
{
    [QueryProperty(nameof(IdClientUser), "IdClientUser")]
    [QueryProperty(nameof(IdProviderUser), "IdProviderUser")]
    [QueryProperty(nameof(IdTaskRequest), "IdTaskRequest")]
    public partial class ChatConversationPage : ContentPage
    {
        private Guid _idClientUser;
        private Guid _idProviderUser;
        private Guid _idTaskRequest;

        public string IdClientUser
        {
            set
            {
                if (Guid.TryParse(value, out var parsed))
                {
                    _idClientUser = parsed;
                    _clientSet = true;
                    TryInitializeIfReady();
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
                    _providerSet = true;
                    TryInitializeIfReady();
                }
            }
        }

        public string IdTaskRequest
        {
            set
            {
                Console.WriteLine($"[SETTER] IdTaskRequest = {value}");
                if (Guid.TryParse(value, out var parsed))
                {
                    _idTaskRequest = parsed;
                }

                _taskSet = true;
                TryInitializeIfReady();
            }
        }

        private bool _clientSet, _providerSet, _taskSet;

        private async void TryInitializeIfReady()
        {
            if (_clientSet && _providerSet && ViewModel != null)
            {
                ViewModel.IdTaskRequest = _idTaskRequest;
                await ViewModel.LoadMessagesAsync(_idClientUser, _idProviderUser, _idTaskRequest);

                // Esperar para que se rendericen los mensajes
                await Task.Delay(100);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (MessagesList.ItemsSource is IList<ChatMessageDto> messages && messages.Count > 0)
                    {
                        MessagesList.ScrollTo(messages.Last(), position: ScrollToPosition.End, animate: false);
                    }
                });
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
