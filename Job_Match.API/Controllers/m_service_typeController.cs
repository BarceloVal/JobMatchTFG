using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Job_Match.Data.Models;
using Job_Match.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/service-type")]
    public class m_service_typeController : ControllerBase
    {
        #region parameters

        private readonly job_match_context _context;

        public m_service_typeController(job_match_context context)
        {
            _context = context;
        }

        #endregion

        #region methods GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<m_service_type>>> GetAll()
        {
            var list = await _context.m_service_types
                .Where(s => s.active)
                .OrderBy(s => s.name)
                .ToListAsync();

            return Ok(list);
        }

        #endregion
    }
}
