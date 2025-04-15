using Microsoft.EntityFrameworkCore;
using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Services
{
    public class MDocTypeService : IMDocTypeService
    {
        private readonly job_match_context _context;

        public MDocTypeService(job_match_context context)
        {
            _context = context;
        }

        public async Task<List<m_doc_type>> GetAllAsync()
        {
            return await _context.m_doc_types
                                 .Where(d => d.active)
                                 .OrderBy(d => d.name)
                                 .ToListAsync();
        }
    }
}
