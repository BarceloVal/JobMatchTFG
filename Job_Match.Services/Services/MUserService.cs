using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Job_Match.Services.Utils;

namespace Job_Match.Services.Services
{
    public class MUserService(job_match_context context) : IMUserService
    {
        private readonly job_match_context _context = context;

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        public async Task<List<m_user>> GetAllAsync()
        {
            return await _context.m_users.ToListAsync();
        }

        /// <summary>
        /// Obtiene un usuario por su ID, incluyendo sus relaciones con clientes y proveedores.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns> Si existe, o null si no se encuentra.</returns>
        public async Task<m_user?> GetByIdAsync(Guid id)
        {
            return await _context.m_users.Include(x => x.m_user_clients)
                                         .Include(x => x.m_user_providers)
                                         .FirstOrDefaultAsync(x => x.id == id);
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos, aplicando hash a la contraseña.
        /// </summary>
        /// <param name="user">Usuario a crear.</param>
        /// <returns>El usuario creado con ID asignado.</returns>
        public async Task<m_user> CreateAsync(m_user user)
        {
            // Hashear la contraseña antes de guardar
            user.password = PasswordHelper.Hash(user.password);

            _context.m_users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>True si fue eliminado, false si no existe.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.m_users.FindAsync(id);
            if (user == null) return false;

            _context.m_users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Autentica un usuario por su email y contraseña.
        /// </summary>
        /// <param name="mail">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña sin hashear.</param>
        /// <returns>Usuario autenticado si es válido, o null si falla la autenticación.</returns>
        public async Task<m_user?> LoginAsync(string mail, string password)
        {
            var user = await _context.m_users.FirstOrDefaultAsync(u => u.mail == mail);

            if (user == null)
                return null;

            bool isValid = PasswordHelper.Verify(password, user.password);

            return isValid ? user : null;
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="user">Objeto <see cref="m_user"/> con los datos actualizados.</param>
        /// <returns>El usuario actualizado.</returns>
        public async Task<m_user> UpdateAsync(m_user user)
        {
            _context.m_users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
