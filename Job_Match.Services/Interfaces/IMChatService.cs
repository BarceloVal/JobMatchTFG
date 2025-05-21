using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMChatService
    {
        Task<bool> StartChatAsync(StartChatDto dto);
        Task<List<ChatSummaryDto>> GetAllChatsForClientAsync(Guid idClient);
        Task<List<ChatMessageDto>> GetMessagesBetweenUsersAsync(Guid idUser1, Guid idUser2);
        Task<bool> SendMessageAsync(SendMessageDto dto);
        Task<List<ChatSummaryDto>> GetAllChatsForProviderAsync(Guid idProvider);
        Task EnsureContactExistsAsync(Guid idUserClient, Guid idUserProvider, Guid? idTaskRequest);
        Task MarkMessagesAsReadAsync(Guid idUserReader, Guid idUserSender);
    }
}
