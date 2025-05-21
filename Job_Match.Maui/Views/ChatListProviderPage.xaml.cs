using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;
using Job_Match.MAUI.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ChatListProviderPage : ContentPage
{
    public ChatListProviderPage()
    {
        InitializeComponent();
        BindingContext = new ChatListProviderViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ChatNavigationState.HasSentNewMessage)
        {
            ChatNavigationState.HasSentNewMessage = false;
            if (BindingContext is ChatListProviderViewModel vm)
            {
                await vm.LoadChatsAsync();
            }
        }
    }

    private async void btnEnterChat_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (((Button)sender).BindingContext is ChatSummaryDto selectedChat)
            {
                var uri = $"{nameof(ChatConversationPage)}" +
                  $"?IdClientUser={selectedChat.IdUserClient}" +
                  $"&IdProviderUser={UserSessionService.Instance.IdUserProvider}" +
                  $"&IdTaskRequest={(selectedChat.IdTaskRequest?.ToString() ?? Guid.Empty.ToString())}";

                await Shell.Current.GoToAsync(uri);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }
}