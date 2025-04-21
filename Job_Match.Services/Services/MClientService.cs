using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job_Match.Data.Context;
using Microsoft.EntityFrameworkCore;
using Job_Match.API.Models.Dtos;

namespace Job_Match.Services.Services
{
    public class MClientService : IMClientService
    {
        private readonly job_match_context _context;
        private readonly PasswordHasher<m_user> _hasher;

        public MClientService(job_match_context context)
        {
            _context = context;
            _hasher = new PasswordHasher<m_user>();
        }

        public async Task<bool> CreateClientAsync(CreateClientDto dto)
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
                is_client = true,
                is_provider = false,
                active = true,
                created_on = DateTime.UtcNow
            };

            user.password = _hasher.HashPassword(user, dto.Password);

            var client = new m_user_client
            {
                id = Guid.NewGuid(),
                id_user = user.id,
                name = $"{dto.FirstName} {dto.LastName}",
                id_doc_type = dto.DocTypeId,
                num_doc = dto.Document,
                address = dto.Address,
                id_postal_code = dto.PostalCodeId,
                phone_number_1 = dto.Phone1,
                phone_number_2 = dto.Phone2,
                mail = dto.Mail,
                active = true,
                created_on = DateTime.UtcNow
            };

            Console.WriteLine($"--- CREACIÓN USUARIO ---");
            Console.WriteLine($"DocTypeId: {dto.DocTypeId}");
            Console.WriteLine($"Existe en m_doc_type: {await _context.m_doc_types.AnyAsync(x => x.id == dto.DocTypeId)}");

            Console.WriteLine($"PostalCodeId: {dto.PostalCodeId}");
            Console.WriteLine($"Existe en m_postal_code: {await _context.m_postal_codes.AnyAsync(x => x.id == dto.PostalCodeId)}");


            _context.m_users.Add(user);
            _context.m_user_clients.Add(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
