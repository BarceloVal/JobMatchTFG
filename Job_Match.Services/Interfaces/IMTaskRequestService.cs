using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMTaskRequestService
    {
        Task<bool> CreateTaskRequestAsync(CreateTaskRequestDto dto);
        Task<List<TaskRequestWithProvidersDto>> GetAllOffersByClientAsync(Guid idUserClient);
        Task<TaskRequestWithProvidersDto?> GetOfferDetailsAsync(Guid offerId);

    }
}
