using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-request")]
    public class t_task_requestController : ControllerBase
    {
        private readonly IMTaskRequestService _service;
        private readonly job_match_context _context;

        public t_task_requestController(IMTaskRequestService service, job_match_context context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto dto)
        {
            if (dto == null) return BadRequest();

            try
            {
                var result = await _service.CreateTaskRequestAsync(dto);
                return result ? Ok() : StatusCode(500, "Error al crear la solicitud.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR desde el servidor: {ex.Message}");
            }
        }

        [HttpGet("client/{idUserClient}")]
        public async Task<ActionResult<List<TaskRequestWithProvidersDto>>> GetByClient(Guid idUserClient)
        {
            var offers = await _service.GetAllOffersByClientAsync(idUserClient);
            return Ok(offers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskRequestWithProvidersDto>> GetById(Guid id)
        {
            var offer = await _service.GetOfferDetailsAsync(id);
            return offer == null ? NotFound() : Ok(offer);
        }

        [HttpGet("active-by-client/{idUserClient}")]
        public async Task<ActionResult<List<TaskRequestWithProvidersDto>>> GetActiveByClient(Guid idUserClient)
        {
            var offers = await _service.GetAllOffersByClientAsync(idUserClient);
            var activeOffers = offers.Where(o => o.Status.ToLower() == "abierta").ToList();
            return Ok(activeOffers);
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var offer = await _context.t_task_requests.FindAsync(id);
            if (offer == null) return NotFound();

            offer.active = false;
            offer.last_update = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
