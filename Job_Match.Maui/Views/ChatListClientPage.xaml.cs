using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.MAUI.ViewModels;

namespace Job_Match.Maui.Views
{
    public partial class ChatListClientPage : ContentPage
    {
        private ChatListClientViewModel ViewModel => BindingContext as ChatListClientViewModel;

        public ChatListClientPage()
        {
            InitializeComponent();
            BindingContext = new ChatListClientViewModel();
            Loaded += ChatListClientPage_Loaded;
        }

        private async void ChatListClientPage_Loaded(object sender, EventArgs e)
        {
            ViewModel.LoadChatsCommand.Execute(null);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Seleccionaste un chat.");

            if (e.CurrentSelection.FirstOrDefault() is ChatSummaryDto selectedChat)
            {
                await Shell.Current.GoToAsync(nameof(ChatConversationPage), new Dictionary<string, object>
                {
                    { "IdClientUser", UserSessionService.Instance.Id },
                    { "IdProviderUser", selectedChat.IdUserProvider }
                });

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).BindingContext is ChatSummaryDto selectedChat)
                {
                    await Shell.Current.GoToAsync(nameof(ChatConversationPage), new Dictionary<string, object>
            {
                { "IdClientUser", UserSessionService.Instance.Id },
                { "IdProviderUser", selectedChat.IdUserProvider }
            });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
