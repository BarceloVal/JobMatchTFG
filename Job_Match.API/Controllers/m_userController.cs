using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Utils;
using Job_Match.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Job_Match.API.Controllers
{
    [ApiController]
    [Route("api/m_user")]
    [Authorize]
    public class m_userController : ControllerBase
    {

        #region parameters

        private readonly IMUserService _userService;
        private readonly IConfiguration _config;

        public m_userController(IMUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        #endregion

        #region methods GET

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            users = [.. users.OrderBy(x => x.first_name).ThenBy(x => x.last_name)];

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        #endregion

        #region methods POST

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] m_user user)
        {
            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { created.id }, created);
        }

        #endregion

        #region methods PUT

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] m_user updatedUser)
        {
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            existingUser.first_name = updatedUser.first_name;
            existingUser.last_name = updatedUser.last_name;
            existingUser.is_provider = updatedUser.is_provider;
            existingUser.is_client = updatedUser.is_client;
            existingUser.last_update = DateTime.Now;

            var result = await _userService.UpdateAsync(existingUser);

            return Ok(result);
        }


        #endregion

        #region methods DELETE

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _userService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        #endregion

        #region methods LOGIN

        [AllowAnonymous]    // Esto es para que no me pida token, este es el único método que no lo necesita, ya que lo genera
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _userService.LoginAsync(login.mail, login.password);
            if (user == null)
                return Unauthorized("Credenciales inválidas.");

            var token = JwtHelper.GenerateToken(user, _config);

            return Ok(new
            {
                token,
                user = new
                {
                    user.id,
                    user.id_mask,
                    user.mail,
                    user.first_name,
                    user.last_name,
                    user.is_provider,
                    user.is_client,
                    user.last_access,
                    user.active,
                    user.created_on,
                    user.last_update
                }
            });
        }

        #endregion

    }
}
