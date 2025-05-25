using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-score")]
    public class t_task_scoreController : ControllerBase
    {
        #region parameters

        private readonly IMTaskScoreService _service;

        public t_task_scoreController(IMTaskScoreService service)
        {
            _service = service;
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

        #endregion+
    }
}
