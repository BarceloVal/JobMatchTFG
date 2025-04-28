using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Job_Match.Data;
using Job_Match.Data.Context;

namespace Job_Match.Services.Services
{
    public class MServiceProviderService : IMServiceProviderService
    {
        private readonly job_match_context _context;

        public MServiceProviderService(job_match_context context)
        {
            _context = context;
        }

        public async Task<List<m_service_provider>> GetAllServiceProvidersAsync()
        {
            return await _context.m_service_providers
                .Where(sp => sp.active)
                .ToListAsync();
        }

        public async Task AddServicesToProviderAsync(Guid providerId, List<Guid> serviceIds)
        {
            // Obtener el número actual de relaciones para calcular el nuevo id_mask
            int currentCount = await _context.m_service_providers.CountAsync();

            foreach (var serviceId in serviceIds)
            {
                var exists = await _context.m_service_providers
                    .AnyAsync(x => x.id_user_provider == providerId && x.id_service == serviceId);

                if (!exists)
                {
                    currentCount++; // Incrementa para cada nueva relación

                    var newRelation = new m_service_provider
                    {
                        id_user_provider = providerId,
                        id_service = serviceId,
                        id_mask = $"SERPROV{currentCount.ToString("D3")}", // D3 -> número con 3 dígitos (001, 002, 003...)
                        active = true,
                        created_on = DateTime.UtcNow,
                        last_update = null
                    };

                    _context.m_service_providers.Add(newRelation);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
