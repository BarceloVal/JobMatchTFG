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
                id_mask = await GenerarNuevoIdMaskAsync(),
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

        public async Task<bool> ChangeClientToProviderAsync(ChangeClientToProviderDto dto)
        {
            var user = await _context.m_users.FindAsync(dto.IdUser);
            if (user == null)
                return false;

            // 1. Desactivar cliente
            var client = await _context.m_user_clients.FirstOrDefaultAsync(c => c.id_user == dto.IdUser);
            if (client != null)
            {
                client.active = false;
                _context.m_user_clients.Update(client);
            }

            // 2. Verificar si ya es proveedor
            var existingProvider = await _context.m_user_providers.FirstOrDefaultAsync(p => p.id_user == dto.IdUser);

            if (existingProvider != null)
            {
                // Ya existe → solo activar
                existingProvider.active = true;
                _context.m_user_providers.Update(existingProvider);
            }
            else
            {
                var provider = new m_user_provider
                {
                    id = Guid.NewGuid(),
                    id_user = dto.IdUser,
                    trade_name = dto.TradeName,
                    description = dto.Description,
                    mail = user.mail,
                    web_site = dto.Website,
                    map_coordinates = dto.MapCoordinates,
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

                _context.m_user_providers.Add(provider);

                if (dto.IdService.HasValue)
                {
                    var serviceProvider = new m_service_provider
                    {
                        id_user_provider = provider.id,
                        id_service = dto.IdService.Value,
                        created_on = DateTime.UtcNow,
                        last_update = DateTime.UtcNow,
                        active = true,
                        id_mask = await GenerarNuevoServiceProviderIdMaskAsync()
                    };

                    _context.m_service_providers.Add(serviceProvider);
                }
            }

            // 3. Actualizar flags en m_user
            user.is_client = false;
            user.is_provider = true;
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

        private async Task<string> GenerarNuevoServiceProviderIdMaskAsync()
        {
            var ultimo = await _context.m_service_providers
                .Where(x => x.id_mask.StartsWith("SRV"))
                .OrderByDescending(x => x.id_mask)
                .Select(x => x.id_mask)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(ultimo))
                return "SRV001";

            var numero = int.TryParse(ultimo.Substring(3), out var n) ? n + 1 : 1;
            return $"SRV{numero:D3}";
        }

        public async Task<bool> ProviderExistsAsync(Guid idUser)
        {
            return await _context.m_user_providers
                .AnyAsync(p => p.id_user == idUser);
        }
    }
}
