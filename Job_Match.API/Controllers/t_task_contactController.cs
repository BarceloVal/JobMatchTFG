using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [Route("api/task-contacts")]
    [ApiController]
    public class t_task_contactController : ControllerBase
    {
        #region parameters
        private readonly IMChatService _chatService;

        public t_task_contactController(IMChatService chatService)
        {
            _chatService = chatService;
        }

        #endregion

        #region methods POST

        [HttpPost("contact-provider")]
        public async Task<IActionResult> ContactProvider([FromBody] StartChatDto dto)
        {
            var result = await _chatService.StartChatAsync(dto);
            if (!result) return BadRequest("No se pudo iniciar el chat.");
            return Ok("Chat iniciado correctamente.");
        }

        #endregion

        #region methods GET

        [HttpGet("client/{idClient}")]
        public async Task<IActionResult> GetChatsForClient(Guid idClient)
        {
            var result = await _chatService.GetAllChatsForClientAsync(idClient);
            return Ok(result);
        }

        #endregion
    }
}
