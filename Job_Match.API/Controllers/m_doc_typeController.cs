using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Job_Match.Data.Models;
using Job_Match.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/doc-types")]
    public class m_doc_typeController : ControllerBase
    {
        #region parameters
        private readonly job_match_context _context;

        public m_doc_typeController(job_match_context context)
        {
            _context = context;
        }

        #endregion

        #region methods GET

        [HttpGet]
        public async Task<IEnumerable<m_doc_type>> GetDocType()
        {
            return await _context.m_doc_types.ToListAsync();
        }

        #endregion
    }
}
