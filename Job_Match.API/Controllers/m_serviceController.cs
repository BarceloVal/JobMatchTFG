using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class m_serviceController : ControllerBase
    {
        #region parameters

        private readonly job_match_context _context;

        public m_serviceController(job_match_context context)
        {
            _context = context;
        }

        #endregion

        #region methods GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<m_service>>> GetServices()
        {
            var list = await _context.m_services
                .Where(s => s.active)
                .OrderBy(s => s.name)
                .ToListAsync();

            return Ok(list);
        }

        #endregion
    }
}
