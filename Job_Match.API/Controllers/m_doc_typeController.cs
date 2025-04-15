using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Job_Match.Data.Models;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/doc-types")]
    public class m_doc_typeController : ControllerBase
    {
        private readonly IMDocTypeService _docTypeService;

        public m_doc_typeController(IMDocTypeService docTypeService)
        {
            _docTypeService = docTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<m_doc_type>>> GetAll()
        {
            var result = await _docTypeService.GetAllAsync();
            return Ok(result);
        }
    }
}
