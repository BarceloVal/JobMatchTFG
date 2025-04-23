using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Job_Match.Data.Models;
using Job_Match.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/postal-codes")]
    public class m_postal_codeController : ControllerBase
    {
        private readonly job_match_context _context;

        public m_postal_codeController(job_match_context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<m_postal_code>> GetPostalCode()
        {
            return await _context.m_postal_codes.ToListAsync();
        }
    }
}
