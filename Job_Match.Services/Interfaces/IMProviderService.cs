using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMProviderService
    {
        Task<bool> CreateProviderAsync(CreateProviderDto dto);
        Task<m_user_provider?> GetProviderByMailAsync(string mail);

    }
}
