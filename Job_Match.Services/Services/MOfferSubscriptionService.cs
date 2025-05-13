using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Services
{
    public class MOfferSubscriptionService : IMOfferSubscriptionService
    {
        private readonly job_match_context _context;

        public MOfferSubscriptionService(job_match_context context)
        {
            _context = context;
        }

        public async Task SubscribeToOfferAsync(OfferSubscriptionDto dto)
        {
            try
            {
                Console.WriteLine($"DTO recibido: {dto.IdTaskRequest} - {dto.IdUserProvider}");

                var exists = await _context.t_offer_subscriptions
                    .AnyAsync(x => x.id_task_request == dto.IdTaskRequest && x.id_user_provider == dto.IdUserProvider);

                if (exists)
                {
                    Console.WriteLine("Ya existe suscripción.");
                    return;
                }

                var taskExists = await _context.t_task_requests.AnyAsync(x => x.id == dto.IdTaskRequest);
                var providerExists = await _context.m_user_providers.AnyAsync(x => x.id == dto.IdUserProvider);

                if (!taskExists || !providerExists)
                {
                    throw new Exception("El id_task_request o id_user_provider no existe.");
                }

                var subscription = new t_offer_subscription
                {
                    id_task_request = dto.IdTaskRequest,
                    id_user_provider = dto.IdUserProvider,
                    active = true,
                    created_on = DateTime.Now,
                    last_update = null
                };

                Console.WriteLine($"Enviando: {dto.IdTaskRequest} - {dto.IdUserProvider}");
                Console.WriteLine($"Guardando: active={subscription.active}");

                _context.t_offer_subscriptions.Add(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR SubscribeToOfferAsync] {ex.Message}");
                throw;
            }
        }
    }
}
