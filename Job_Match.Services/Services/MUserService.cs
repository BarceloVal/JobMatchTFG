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
        /// Devuelve una lista de objetos del tipo m_user (todos)
        /// </summary>
        /// <returns></returns>
        public async Task<List<m_user>> GetAllAsync()
        {
            return await _context.m_users.ToListAsync();
        }

        /// <summary>
        /// Devuelve un objeto del tipo m_user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<m_user?> GetByIdAsync(Guid id)
        {
            return await _context.m_users.FindAsync(id);
        }

        /// <summary>
        /// Crea un usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<m_user> CreateAsync(m_user user)
        {
            // Hashear la contraseña antes de guardar
            user.password = PasswordHelper.Hash(user.password);

            _context.m_users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Borra un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.m_users.FindAsync(id);
            if (user == null) return false;

            _context.m_users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Login de Usuario
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<m_user?> LoginAsync(string mail, string password)
        {
            var user = await _context.m_users.FirstOrDefaultAsync(u => u.mail == mail);

            if (user == null)
                return null;

            bool isValid = PasswordHelper.Verify(password, user.password);

            return isValid ? user : null;
        }

    }
}
