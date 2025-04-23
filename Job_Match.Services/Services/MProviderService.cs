using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Services.Services
{
    public class MProviderService : IMProviderService
    {
        private readonly job_match_context _context;
        private readonly PasswordHasher<m_user> _hasher;

        public MProviderService(job_match_context context)
        {
            _context = context;
            _hasher = new PasswordHasher<m_user>();
        }

        public async Task<bool> CreateProviderAsync(CreateProviderDto dto)
        {
            if (await _context.m_users.AnyAsync(u => u.mail == dto.Mail))
                return false;

            var user = new m_user
            {
                id = Guid.NewGuid(),
                id_mask = Guid.NewGuid().ToString("N").Substring(0, 10),
                mail = dto.Mail,
                first_name = dto.FirstName,
                last_name = dto.LastName,
                is_client = false,
                is_provider = true,
                active = true,
                created_on = DateTime.UtcNow,
                last_update = DateTime.UtcNow,
            };

            user.password = _hasher.HashPassword(user, dto.Password);

            var provider = new m_user_provider
            {
                id = Guid.NewGuid(),
                id_user = user.id,
                trade_name = dto.TradeName,
                description = dto.Description,
                id_doc_type = dto.IdDocType,
                num_doc = dto.Document,
                address = dto.Address,
                mail = user.mail,
                web_site = dto.Website,
                id_postal_code = dto.IdPostalCode,
                phone_number_1 = dto.Phone1,
                phone_number_2 = dto.Phone2,
                map_coordinates = dto.MapCoordinates,
                created_on = DateTime.Now,
                last_update = DateTime.Now,
                active = true
            };

            _context.m_users.Add(user);
            _context.m_user_providers.Add(provider);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<m_user_provider?> GetProviderByMailAsync(string mail)
        {
            return await _context.m_user_providers
                .Include(p => p.id_userNavigation)
                .Include(p => p.id_postal_codeNavigation)
                .FirstOrDefaultAsync(p => p.mail == mail);
        }

    }
}
