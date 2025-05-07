using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-request")]
    public class t_task_requestController : ControllerBase
    {
        private readonly IMTaskRequestService _service;

        public t_task_requestController(IMTaskRequestService service)
        {
            _service = service;
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
    }
}
