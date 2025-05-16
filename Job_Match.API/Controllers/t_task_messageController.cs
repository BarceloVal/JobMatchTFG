using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/task-messages")]
    public class t_task_messageController : ControllerBase
    {
        #region parameters

        private readonly IMChatService _chatService;

        public t_task_messageController(IMChatService chatService)
        {
            _chatService = chatService;
        }

        #endregion

        #region methods GET

        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation([FromQuery] Guid idUser1, [FromQuery] Guid idUser2)
        {
            if (idUser1 == Guid.Empty || idUser2 == Guid.Empty)
                return BadRequest("IDs no válidos");

            var result = await _chatService.GetMessagesBetweenUsersAsync(idUser1, idUser2);

            return Ok(result);
        }

        [HttpGet("between/{idUser1}/{idUser2}")]
        public async Task<IActionResult> GetMessages(Guid idUser1, Guid idUser2)
        {
            var result = await _chatService.GetMessagesBetweenUsersAsync(idUser1, idUser2);
            return Ok(result);
        }

        #endregion

        #region methods POST

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            var success = await _chatService.SendMessageAsync(dto);
            if (!success) return BadRequest("No se pudo enviar el mensaje.");
            return Ok();
        }

        #endregion
    }
}
