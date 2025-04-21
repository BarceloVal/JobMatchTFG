using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job_Match.API.Models.Dtos;

namespace Job_Match.Services.Interfaces
{
    public interface IMClientService
    {
        Task<bool> CreateClientAsync(CreateClientDto dto);
    }

}
