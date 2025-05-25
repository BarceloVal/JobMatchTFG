using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-score")]
    public class t_task_scoreController : ControllerBase
    {
        #region parameters

        private readonly IMTaskScoreService _service;
        private readonly job_match_context _context;

        public t_task_scoreController(IMTaskScoreService service, job_match_context context)
        {
            _service = service;
            _context = context;
        }

        #endregion

        #region methods POST

        [HttpPost]
        public async Task<IActionResult> CreateScore([FromBody] CreateTaskScoreDto dto)
        {
            try
            {
                Console.WriteLine($"Creando score: Reviewer={dto.IdUserReviewer}, Reviewed={dto.IdUserReviewed}, Rating={dto.Rating}");
                await _service.CreateScoreAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar score: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region methods GET

        [HttpGet("received/{idUser}")]
        public async Task<ActionResult<IEnumerable<TaskScoreDisplayDto>>> GetScoresReceivedByUser(Guid idUser)
        {
            var scores = await _context.t_task_scores
                .Where(s => s.id_user_reviewed == idUser && s.active)
                .Include(s => s.id_user_reviewerNavigation)
                .Select(s => new TaskScoreDisplayDto
                {
                    Rating = s.rating,
                    Comment = s.comment,
                    CreatedOn = s.created_on,
                    ReviewerName = s.id_user_reviewerNavigation.first_name + " " + s.id_user_reviewerNavigation.last_name,
                    IdUserReviewer = s.id_user_reviewer,
                    IdUserReviewed = s.id_user_reviewed
                })
                .OrderByDescending(s => s.CreatedOn)
                .ToListAsync();

            return Ok(scores);
        }

        #endregion

        #region methods PUT

        [HttpPut("deactivate/{idReviewer}/{idReviewed}")]
        public async Task<IActionResult> DeactivateScore(Guid idReviewer, Guid idReviewed)
        {
            var score = await _context.t_task_scores
                .FirstOrDefaultAsync(s =>
                    s.id_user_reviewer == idReviewer &&
                    s.id_user_reviewed == idReviewed &&
                    s.active);

            if (score == null)
                return NotFound();

            score.active = false;
            score.last_update = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion
    }
}
