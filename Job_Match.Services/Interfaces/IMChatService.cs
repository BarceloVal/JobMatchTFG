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
    }
}
