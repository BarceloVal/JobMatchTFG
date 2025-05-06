using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job_Match.API.Models.Dtos;
using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;

namespace Job_Match.Services.Interfaces
{
    public interface IMClientService
    {
        Task<bool> CreateClientAsync(CreateClientDto dto);
        Task<m_user_client?> GetClientByMailAsync(string mail);
        Task<bool> ChangeProviderToClientAsync(ChangeProviderToClientDto dto);
        Task<bool> ClientExistsAsync(Guid idUser);
    }

}
