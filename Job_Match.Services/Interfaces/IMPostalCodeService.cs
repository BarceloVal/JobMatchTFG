using Job_Match.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Interfaces
{
    public interface IMPostalCodeService
    {
        Task<List<string>> GetAllAsStringAsync();
    }
}
