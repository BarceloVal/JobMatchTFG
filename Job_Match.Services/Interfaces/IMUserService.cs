using Job_Match.Data.Models;

namespace Job_Match.Services.Interfaces
{
    public interface IMUserService
    {
        Task<List<m_user>> GetAllAsync();
        Task<m_user?> GetByIdAsync(Guid id);
        Task<m_user> CreateAsync(m_user user);
        Task<bool> DeleteAsync(Guid id);
        Task<m_user?> LoginAsync(string mail, string password);
        Task<m_user> UpdateAsync(m_user user);

    }
}
