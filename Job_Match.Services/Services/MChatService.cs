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
            var userClient = await _context.m_user_clients
                .FirstOrDefaultAsync(x => x.id_user == dto.IdUserClient);

            var userProvider = await _context.m_user_providers
                .FirstOrDefaultAsync(x => x.id_user == dto.IdUserProvider);

            Console.WriteLine($"[StartChatAsync] Client encontrado: {userClient != null}, Provider encontrado: {userProvider != null}");

            if (userClient == null || userProvider == null) return false;

            // Verificar o crear contacto
            var contact = await _context.t_task_contacts.FirstOrDefaultAsync(x =>
                x.id_user_client == userClient.id &&
                x.id_user_provider == userProvider.id &&
                (
                    (dto.IdTaskRequest == null && x.id_task_request == null) ||
                    (dto.IdTaskRequest != null && x.id_task_request == dto.IdTaskRequest)
                ));

            if (contact == null)
            {
                // Crear nuevo contacto si no existe
                contact = new t_task_contact
                {
                    id = Guid.NewGuid(),
                    id_user_client = userClient.id,
                    id_user_provider = userProvider.id,
                    id_task_request = dto.IdTaskRequest,
                    can_chat = true,
                    active = true,
                    created_on = DateTime.Now,
                    last_update = DateTime.Now
                };

                _context.t_task_contacts.Add(contact);
                Console.WriteLine("[StartChatAsync] Contacto creado.");
            }
            else
            {
                contact.can_chat = true;
                contact.last_update = DateTime.Now;
                Console.WriteLine("[StartChatAsync] Contacto actualizado (ya existía).");
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
                Console.WriteLine("[StartChatAsync] Mensaje creado.");
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
                    IdUserProvider = provider?.id_user ?? Guid.Empty,
                    ProviderName = provider?.id_userNavigation?.first_name + " " + provider?.id_userNavigation?.last_name,
                    LastMessage = lastMessage?.message,
                    LastMessageDate = lastMessage?.sent_on
                };
            }).ToList();

            return result;
        }

        public async Task<List<ChatMessageDto>> GetMessagesBetweenUsersAsync(Guid idUser1, Guid idUser2)
        {
            Console.WriteLine($"[Buscar Mensajes] idUser1: {idUser1}, idUser2: {idUser2}");

            var messages = await _context.t_task_messages
                .Where(m =>
                ((m.id_user_sender == idUser1 && m.id_user_reciver == idUser2) ||
                 (m.id_user_sender == idUser2 && m.id_user_reciver == idUser1)) &&
                 m.active == true)
                .OrderBy(m => m.sent_on)
                .ToListAsync();

            Console.WriteLine($"[Mensajes encontrados]: {messages.Count}");

            var users = await _context.m_users
                .Where(u => u.id == idUser1 || u.id == idUser2)
                .ToListAsync();

            var result = messages.Select(m =>
            {
                var sender = users.FirstOrDefault(u => u.id == m.id_user_sender);

                return new ChatMessageDto
                {
                    Id = m.id,
                    IdSender = m.id_user_sender,
                    SenderName = sender != null ? $"{sender.first_name} {sender.last_name}" : "",
                    Message = m.message,
                    SentOn = m.sent_on,
                    IsMine = m.id_user_sender == idUser1
                };
            }).ToList();

            return result;
        }

        // ENviar mensaje
        public async Task<bool> SendMessageAsync(SendMessageDto dto)
        {
            var message = new t_task_message
            {
                id = Guid.NewGuid(),
                id_user_sender = dto.IdSender,
                id_user_reciver = dto.IdReceiver,
                id_task_request = (dto.IdTaskRequest == Guid.Empty) ? null : dto.IdTaskRequest,
                message = dto.Message,
                sent_on = DateTime.Now,
                created_on = DateTime.Now,
                last_update = DateTime.Now,
                active = true
            };

            _context.t_task_messages.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChatSummaryDto>> GetAllChatsForProviderAsync(Guid idProvider)
        {
            var chats = await _context.t_task_contacts
                .Where(c => c.id_user_provider == idProvider && c.active && c.can_chat)
                .Include(c => c.id_task_requestNavigation)
                    .ThenInclude(r => r.id_user_clientNavigation)
                .ToListAsync();

            var result = new List<ChatSummaryDto>();

            foreach (var chat in chats)
            {
                var idTaskRequest = chat.id_task_request;

                var lastMessage = await _context.t_task_messages
                    .Where(m => m.id_task_request == idTaskRequest)
                    .OrderByDescending(m => m.sent_on)
                    .FirstOrDefaultAsync();

                var client = chat.id_task_requestNavigation.id_user_clientNavigation;

                result.Add(new ChatSummaryDto
                {
                    IdUserClient = client.id,
                    ClientName = client.name,
                    LastMessage = lastMessage?.message ?? "(sin mensajes)",
                    LastMessageDate = lastMessage?.sent_on ?? chat.created_on
                });
            }

            return result.OrderByDescending(c => c.LastMessageDate).ToList();
        }

        public async Task EnsureContactExistsAsync(Guid idUserClient, Guid idUserProvider, Guid? idTaskRequest)
        {
            // Obtener el m_user_client y m_user_provider desde el id_user
            var client = await _context.m_user_clients.FirstOrDefaultAsync(c => c.id_user == idUserClient);
            var provider = await _context.m_user_providers.FirstOrDefaultAsync(p => p.id_user == idUserProvider);

            if (client == null || provider == null)
            {
                Console.WriteLine("[EnsureContactExistsAsync] No se encontró el cliente o proveedor.");
                return;
            }

            // Verificar si ya existe el contacto
            var exists = await _context.t_task_contacts.AnyAsync(c =>
                c.id_user_client == client.id &&
                c.id_user_provider == provider.id &&
                (
                    (idTaskRequest == null || idTaskRequest == Guid.Empty) && c.id_task_request == null ||
                    (idTaskRequest != null && idTaskRequest != Guid.Empty && c.id_task_request == idTaskRequest)
                )
            );

            if (!exists)
            {
                var newContact = new t_task_contact
                {
                    id = Guid.NewGuid(),
                    id_user_client = client.id,
                    id_user_provider = provider.id,
                    id_task_request = idTaskRequest,
                    can_chat = true,
                    active = true,
                    created_on = DateTime.Now,
                    last_update = DateTime.Now
                };

                _context.t_task_contacts.Add(newContact);
                await _context.SaveChangesAsync();

                Console.WriteLine("[EnsureContactExistsAsync] Contacto creado.");
            }
            else
            {
                Console.WriteLine("[EnsureContactExistsAsync] Ya existía el contacto.");
            }
        }
    }
}
