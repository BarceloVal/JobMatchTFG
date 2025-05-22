using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Services
{
    public class MTaskStatusService : IMTaskStatusService
    {
        private readonly job_match_context _context;

        public MTaskStatusService(job_match_context context)
        {
            _context = context;
        }

        public async Task<List<m_task_status>> GetAllAsync()
        {
            return await _context.m_task_statuses
                .Where(s => s.active)
                .OrderBy(s => s.name)
                .ToListAsync();
        }

        public async Task<List<TaskStatusDto>> GetAllTaskStatusesAsync()
        {
            return await _context.m_task_statuses
                .Where(s => s.active)
                .OrderBy(s => s.name)
                .Select(s => new TaskStatusDto
                {
                    Id = s.id,
                    Name = s.name
                })
                .ToListAsync();
        }
    }
}
