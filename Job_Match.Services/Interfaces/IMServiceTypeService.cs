using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMServiceTypeService
    {
        Task<List<string>> GetAllServiceTypesAsStringAsync();
    }
}
