using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-status")]
    public class m_task_statusController : ControllerBase
    {
        private readonly IMTaskStatusService _service;

        public m_task_statusController(IMTaskStatusService service)
        {
            _service = service;
        }

        // GET: api/taskstatus
        [HttpGet]
        public async Task<ActionResult<List<m_task_status>>> GetAll()
        {
            var statuses = await _service.GetAllAsync();
            return Ok(statuses);
        }
    }
}
