using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ChatListProviderPage : ContentPage
{
    public ChatListProviderPage()
    {
        InitializeComponent();
        BindingContext = new ChatListProviderViewModel();
    }

    private async void btnEnterChat_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (((Button)sender).BindingContext is ChatSummaryDto selectedChat)
            {
                var uri = $"{nameof(ChatConversationPage)}" +
                  $"?IdClientUser={UserSessionService.Instance.Id}" +
                  $"&IdProviderUser={selectedChat.IdUserProvider}" +
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