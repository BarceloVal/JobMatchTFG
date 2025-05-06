using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Job_Match.Services.Utils;
using Job_Match.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Job_Match.Data.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Job_Match.Data.Context;
using Job_Match.Data;

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
        private readonly job_match_context _context;

        public m_userController(IMUserService userService, IConfiguration config, job_match_context context)
        {
            _userService = userService;
            _config = config;
            _context = context;
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

        [HttpPost("disable")]
        public async Task<IActionResult> DisableAccount([FromBody] DisableAccountDto dto)
        {
            Console.WriteLine($"Recibido ID: {dto.IdUser}");

            var user = await _context.m_users.FindAsync(dto.IdUser);
            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado");
                return NotFound("Usuario no encontrado");
            }

            // Obtener ID del proveedor asociado al usuario
            var provider = await _context.m_user_providers
                .FirstOrDefaultAsync(x => x.id_user == dto.IdUser);

            // Desactivar al proveedor y su servicio asociado
            if (provider != null)
            {
                var sp = await _context.m_service_providers
                    .FirstOrDefaultAsync(x => x.id_user_provider == provider.id);

                if (sp != null)
                {
                    sp.active = false;
                    sp.last_update = DateTime.Now;

                    _context.Entry(sp).State = EntityState.Modified;
                }

                provider.active = false;
            }

            user.active = false;
            user.is_client = false;
            user.is_provider = false;

            // Desactivar cliente
            var client = await _context.m_user_clients.FirstOrDefaultAsync(x => x.id_user == dto.IdUser);
            if (client != null) client.active = false;

            await _context.SaveChangesAsync();

            Console.WriteLine("Cuenta desactivada correctamente");
            return Ok();
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

            if (!user.active)
                return Unauthorized("Esta cuenta está desactivada. Si deseas reactivarla, contacta con soporte o crea una nueva cuenta.");

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
