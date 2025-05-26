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

                var existingSubscription = await _context.t_offer_subscriptions
                    .FirstOrDefaultAsync(x =>
                        x.id_task_request == dto.IdTaskRequest &&
                        x.id_user_provider == dto.IdUserProvider);

                if (existingSubscription != null)
                {
                    if (existingSubscription.active)
                    {
                        Console.WriteLine("Ya existe suscripción activa.");
                        return; // Ya está activo, no hacemos nada
                    }

                    // Reactivar la suscripción existente
                    Console.WriteLine("Reactivando suscripción existente.");
                    existingSubscription.active = true;
                    existingSubscription.last_update = DateTime.UtcNow;
                    _context.t_offer_subscriptions.Update(existingSubscription);
                }
                else
                {
                    // Verificar que existan las entidades relacionadas
                    var taskExists = await _context.t_task_requests.AnyAsync(x => x.id == dto.IdTaskRequest);
                    var providerExists = await _context.m_user_providers.AnyAsync(x => x.id == dto.IdUserProvider);

                    if (!taskExists || !providerExists)
                        throw new Exception("El id_task_request o id_user_provider no existe.");

                    var subscription = new t_offer_subscription
                    {
                        id_task_request = dto.IdTaskRequest,
                        id_user_provider = dto.IdUserProvider,
                        active = true,
                        created_on = DateTime.UtcNow,
                        last_update = null
                    };

                    Console.WriteLine("Creando nueva suscripción.");
                    _context.t_offer_subscriptions.Add(subscription);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR SubscribeToOfferAsync] {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsAlreadySubscribedAsync(OfferSubscriptionDto dto)
        {
            return await _context.t_offer_subscriptions
                .AnyAsync(x =>
                    x.id_task_request == dto.IdTaskRequest &&
                    x.id_user_provider == dto.IdUserProvider &&
                    x.active);
        }

        public async Task<List<SubscribedOfferDto>> GetOffersByProviderAsync(Guid idUserProvider)
        {
            return await _context.t_offer_subscriptions
                .Where(os => os.id_user_provider == idUserProvider && os.active)
                .Include(os => os.id_task_requestNavigation)
                    .ThenInclude(tr => tr.id_statusNavigation)
                .Include(os => os.id_task_requestNavigation.id_postal_codeNavigation)
                .Select(os => new SubscribedOfferDto
                {
                    IdOffer = os.id_task_request,
                    Title = os.id_task_requestNavigation.title,
                    Description = os.id_task_requestNavigation.description,
                    Status = os.id_task_requestNavigation.id_statusNavigation.name,
                    Active = os.id_task_requestNavigation.active,
                    DateStart = os.id_task_requestNavigation.date_start,
                    DateEnd = os.id_task_requestNavigation.date_end,
                    PostalCodeName = os.id_task_requestNavigation.id_postal_codeNavigation.DisplayText
                }).ToListAsync();
        }

        public async Task<bool> QuitOfferAsync(QuitOfferDto dto)
        {
            var record = await _context.t_offer_subscriptions.FirstOrDefaultAsync(x =>
                x.id_user_provider == dto.IdUserProvider &&
                x.id_task_request == dto.IdTaskRequest &&
                x.active);

            if (record == null)
                return false;

            record.active = false;
            record.last_update = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
