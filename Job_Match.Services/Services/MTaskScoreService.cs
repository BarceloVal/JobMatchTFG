using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job_Match.Data.Context;

namespace Job_Match.Services.Services
{
    public class MTaskScoreService : IMTaskScoreService
    {
        private readonly job_match_context _context;

        public MTaskScoreService(job_match_context context)
        {
            _context = context;
        }

        public async Task CreateScoreAsync(CreateTaskScoreDto dto)
        {
            var score = new t_task_score
            {
                id_user_reviewer = dto.IdUserReviewer,
                id_user_reviewed = dto.IdUserReviewed,
                rating = dto.Rating,
                comment = dto.Comment,
                active = true,
                created_on = DateTime.UtcNow,
                last_update = null
            };

            _context.t_task_scores.Add(score);
            await _context.SaveChangesAsync();
        }
    }

}
