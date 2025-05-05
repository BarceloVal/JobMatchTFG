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
using Job_Match.Data.Models.Dtos;

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
                id_mask = await GenerarNuevoIdMaskAsync(),
                mail = dto.Mail,
                first_name = dto.FirstName,
                last_name = dto.LastName,
                is_client = true,
                is_provider = false,
                active = true,
                created_on = DateTime.UtcNow,
                last_update = DateTime.UtcNow,
            };

            user.password = _hasher.HashPassword(user, dto.Password);

            var client = new m_user_client
            {
                id = Guid.NewGuid(),
                id_user = user.id,
                name = $"{dto.FirstName} {dto.LastName}",
                id_doc_type = dto.IdDocType,
                num_doc = dto.Document,
                address = dto.Address,
                mail = user.mail,
                id_postal_code = dto.IdPostalCode,
                phone_number_1 = dto.Phone1,
                phone_number_2 = dto.Phone2,
                active = true,
                created_on = DateTime.UtcNow,
                last_update = DateTime.UtcNow
            };

            _context.m_users.Add(user);
            _context.m_user_clients.Add(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<m_user_client?> GetClientByMailAsync(string mail)
        {
            return await _context.m_user_clients
                .Include(c => c.id_userNavigation)
                .Include(c => c.id_postal_codeNavigation)
                .FirstOrDefaultAsync(c => c.mail == mail);
        }

        /// <summary>
        /// Genera un nuevo valor de id_mask en formato "USR001", "USR002", etc., 
        /// incrementando el último valor existente en la base de datos.
        /// </summary>
        /// <returns>Nuevo id_mask con el formato USR###</returns>
        private async Task<string> GenerarNuevoIdMaskAsync()
        {
            // Busca el último id_mask que empiece con "USR" y lo ordena en forma descendente
            var ultimoIdMask = await _context.m_users
                .Where(u => u.id_mask.StartsWith("USR"))
                .OrderByDescending(u => u.id_mask)
                .Select(u => u.id_mask)
                .FirstOrDefaultAsync();

            // Si no existe ningún id_mask todavía, devuelve el primero
            if (string.IsNullOrEmpty(ultimoIdMask))
                return "USR001";

            // Extrae la parte numérica del último id_mask (por ejemplo: de "USR007" saca "007")
            var numeroStr = ultimoIdMask.Substring(3);

            // Intenta convertir esa parte numérica a entero
            if (!int.TryParse(numeroStr, out int numero))
                numero = 0;

            // Incrementa el número
            numero++;

            // Devuelve el nuevo id_mask con el número formateado a 3 cifras (ej: "USR008")
            return $"USR{numero:D3}";
        }

        public async Task<bool> ChangeProviderToClientAsync(ChangeProviderToClientDto dto)
        {
            var user = await _context.m_users.FindAsync(dto.IdUser);
            if (user == null)
                return false;

            // 1. Desactivar proveedor
            var provider = await _context.m_user_providers.FirstOrDefaultAsync(c => c.id_user == dto.IdUser);
            if (provider != null)
            {
                provider.active = false;
                _context.m_user_providers.Update(provider);
            }

            // 2. Verificar si ya es cliente
            var existingClient = await _context.m_user_clients.FirstOrDefaultAsync(p => p.id_user == dto.IdUser);

            if (existingClient != null)
            {
                // Ya existe → solo activar
                existingClient.active = true;
                _context.m_user_clients.Update(existingClient);
            }
            else
            {
                var client = new m_user_client
                {
                    id = Guid.NewGuid(),
                    id_user = dto.IdUser,
                    mail = user.mail,
                    id_doc_type = dto.IdDocType.Value,
                    num_doc = dto.Document,
                    address = dto.Address,
                    id_postal_code = dto.IdPostalCode.Value,
                    phone_number_1 = dto.Phone1,
                    phone_number_2 = dto.Phone2,
                    created_on = DateTime.UtcNow,
                    last_update = DateTime.UtcNow,
                    active = true
                };

                _context.m_user_clients.Add(client);
            }

            // 3. Actualizar flags en m_user
            user.is_client = true;
            user.is_provider = false;
            user.last_update = DateTime.UtcNow;

            // Solo actualizar si vienen datos nuevos, para evitar pisar con null
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.first_name = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.last_name = dto.LastName;


            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var hasher = new PasswordHasher<m_user>();
                user.password = hasher.HashPassword(user, dto.Password);
            }

            try
            {
                _context.m_users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Falló SaveChangesAsync: " + ex.Message);
                Console.WriteLine("[ERROR] StackTrace: " + ex.StackTrace);
                return false;
            }

        }

        public async Task<bool> ClientExistsAsync(Guid idUser)
        {
            return await _context.m_user_clients
                .AnyAsync(p => p.id_user == idUser);
        }
    }
}
