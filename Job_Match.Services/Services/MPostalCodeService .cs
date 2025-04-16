using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Services.Services
{
    public class MPostalCodeService : IMPostalCodeService
    {
        private readonly job_match_context _context;

        public MPostalCodeService(job_match_context context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllAsStringAsync()
        {
            return await _context.m_postal_codes
                .Where(p => p.active)
                .OrderBy(p => p.cod_auto_name)
                .ThenBy(p => p.cod_pro_name)
                .ThenBy(p => p.cod_loc_name)
                .Select(p => p.cod_auto_name + "/" + p.cod_pro_name + "/" + p.cod_loc_name)
                .ToListAsync();
        }
    }

}
