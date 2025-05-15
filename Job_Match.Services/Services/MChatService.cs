using Job_Match.Data.Context;
using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Services.Services
{
    public class MChatService : IMChatService
    {
        private readonly job_match_context _context;

        public MChatService(job_match_context context)
        {
            _context = context;
        }

        public async Task<bool> StartChatAsync(StartChatDto dto)
        {
            // Validar que el proveedor y cliente existen
            var userClient = await _context.m_user_clients.FindAsync(dto.IdUserClient);
            var userProvider = await _context.m_user_providers.FindAsync(dto.IdUserProvider);

            if (userClient == null || userProvider == null) return false;

            if (dto.IdTaskRequest != null)
            {
                // Caso: oferta
                var contact = await _context.t_task_contacts.FirstOrDefaultAsync(x =>
                    x.id_user_provider == dto.IdUserProvider &&
                    x.id_task_request == dto.IdTaskRequest);

                if (contact == null) return false;

                contact.can_chat = true;
                contact.last_update = DateTime.Now;
            }

            // Crear mensaje inicial si viene texto
            if (!string.IsNullOrEmpty(dto.InitialMessage))
            {
                var message = new t_task_message
                {
                    id = Guid.NewGuid(),
                    id_task_request = dto.IdTaskRequest,
                    id_user_sender = userClient.id_user,
                    id_user_reciver = userProvider.id_user,
                    message = dto.InitialMessage,
                    sent_on = DateTime.Now,
                    active = true,
                    created_on = DateTime.Now,
                    last_update = DateTime.Now
                };

                await _context.t_task_messages.AddAsync(message);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChatSummaryDto>> GetAllChatsForClientAsync(Guid idClient)
        {
            // Obtener el id_user correspondiente al cliente
            var client = await _context.m_user_clients.FindAsync(idClient);
            if (client == null) return new List<ChatSummaryDto>();

            var userId = client.id_user;

            // 1. Buscar todos los mensajes donde el cliente está como remitente o destinatario
            var messages = await _context.t_task_messages
                .Where(m => m.id_user_sender == userId || m.id_user_reciver == userId)
                .ToListAsync();

            // 2. Extraer IDs únicos de proveedores (usuarios m_user)
            var providerUserIds = messages
                .Select(m => m.id_user_sender == userId ? m.id_user_reciver : m.id_user_sender)
                .Distinct()
                .ToList();

            // 3. Buscar los datos de esos proveedores
            var providers = await _context.m_user_providers
                .Where(p => providerUserIds.Contains(p.id_user))
                .Include(p => p.id_userNavigation)
                .ToListAsync();

            // 4. Armar los DTOs
            var result = providerUserIds.Select(providerUserId =>
            {
                var lastMessage = messages
                    .Where(m =>
                        (m.id_user_sender == userId && m.id_user_reciver == providerUserId) ||
                        (m.id_user_sender == providerUserId && m.id_user_reciver == userId))
                    .OrderByDescending(m => m.sent_on)
                    .FirstOrDefault();

                var provider = providers.FirstOrDefault(p => p.id_user == providerUserId);

                return new ChatSummaryDto
                {
                    IdUserProvider = provider?.id ?? Guid.Empty,
                    ProviderName = provider?.id_userNavigation?.first_name + " " + provider?.id_userNavigation?.last_name,
                    LastMessage = lastMessage?.message,
                    LastMessageDate = lastMessage?.sent_on
                };
            }).ToList();

            return result;
        }
    }
}
