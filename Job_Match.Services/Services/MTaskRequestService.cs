using Job_Match.Data.Context;
using Job_Match.Data.Models.Dtos;
using Job_Match.Data.Models;
using Job_Match.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Services.Services
{
    public class MTaskRequestService : IMTaskRequestService
    {
        private readonly job_match_context _context;

        public MTaskRequestService(job_match_context context)
        {
            _context = context;
        }

        public async Task<bool> CreateTaskRequestAsync(CreateTaskRequestDto dto)
        {
            try
            {
                var entity = new t_task_request
                {
                    id = Guid.NewGuid(),
                    id_user_client = dto.IdUserClient,
                    id_postal_code = dto.IdPostalCode,
                    id_service = dto.IdService,
                    id_status = dto.IdStatus,
                    title = dto.Title,
                    description = dto.Description,
                    date_start = dto.DateStart,
                    date_end = dto.DateEnd,
                    created_on = DateTime.Now,
                    active = true
                };

                _context.t_task_requests.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<TaskRequestWithProvidersDto>> GetAllOffersByClientAsync(Guid idUserClient)
        {
            var offers = await _context.t_task_requests
                .Include(t => t.id_statusNavigation)
                .Include(t => t.id_serviceNavigation)
                    .ThenInclude(s => s.id_service_typeNavigation)
                .Include(t => t.id_postal_codeNavigation)
                .Include(t => t.t_task_contacts)
                    .ThenInclude(c => c.id_user_providerNavigation)
                    .ThenInclude(p => p.id_userNavigation)
                .Where(t => t.id_user_client == idUserClient && t.active)
                .ToListAsync();

            return offers.Select(o => new TaskRequestWithProvidersDto
            {
                Id = o.id,
                Title = o.title ?? "",
                Description = o.description ?? "",
                Status = o.id_statusNavigation.name,
                Service = o.id_serviceNavigation.name,
                ServiceType = o.id_serviceNavigation.id_service_typeNavigation.name,
                PostalCodeName = o.id_postal_codeNavigation.DisplayText,
                Active = o.active,
                DateStart = o.date_start,
                DateEnd = o.date_end,
                Providers = o.t_task_contacts.Select(c => new ProviderTaskRequestDto
                {
                    Id = c.id_user_provider,
                    FullName = $"{c.id_user_providerNavigation.id_userNavigation.first_name} {c.id_user_providerNavigation.id_userNavigation.last_name}",
                    IdTaskRequest = o.id
                }).ToList()
            }).ToList();
        }

        public async Task<TaskRequestWithProvidersDto?> GetOfferDetailsAsync(Guid offerId)
        {
            var o = await _context.t_task_requests
                .Include(t => t.id_statusNavigation)
                .Include(t => t.id_serviceNavigation)
                    .ThenInclude(s => s.id_service_typeNavigation)
                .Include(t => t.id_postal_codeNavigation)
                .Include(t => t.t_task_contacts)
                    .ThenInclude(c => c.id_user_providerNavigation)
                    .ThenInclude(p => p.id_userNavigation)
                .FirstOrDefaultAsync(t => t.id == offerId && t.active);

            if (o == null) return null;

            return new TaskRequestWithProvidersDto
            {
                Id = o.id,
                Title = o.title ?? "",
                Description = o.description ?? "",
                Status = o.id_statusNavigation.name,
                Service = o.id_serviceNavigation.name,
                ServiceType = o.id_serviceNavigation.id_service_typeNavigation.name,
                PostalCodeName = o.id_postal_codeNavigation.DisplayText,
                DateStart = o.date_start,
                DateEnd = o.date_end,
                Active = o.active,
                Providers = o.t_task_contacts.Select(c => new ProviderTaskRequestDto
                {
                    Id = c.id_user_provider,
                    FullName = $"{c.id_user_providerNavigation.id_userNavigation.first_name} {c.id_user_providerNavigation.id_userNavigation.last_name}",
                    IdTaskRequest = o.id
                }).ToList()
            };
        }

        public async Task<bool> UpdateTaskRequestAsync(UpdateTaskRequestDto dto)
        {
            var task = await _context.t_task_requests.FirstOrDefaultAsync(t => t.id == dto.Id && t.active);

            if (task == null)
                return false;

            task.title = dto.Title;
            task.description = dto.Description;
            task.id_postal_code = dto.IdPostalCode;
            task.id_service = dto.IdService;
            task.date_start = dto.DateStart;
            task.id_status = dto.IdStatus;
            task.date_end = dto.DateEnd;
            task.last_update = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskRequestWithOfferDto>> GetOpenOffersAsync()
        {
            var result = await _context.t_task_requests
                .Where(t => t.active && t.id_statusNavigation.name == "Abierta")
                .Include(t => t.id_serviceNavigation)
                    .ThenInclude(s => s.id_service_typeNavigation) // si existe esta relación
                .Include(t => t.id_statusNavigation)
                .Include(t => t.id_postal_codeNavigation)
                .Include(t => t.id_user_clientNavigation)
                .Select(t => new TaskRequestWithOfferDto
                {
                    Id = t.id,
                    Title = t.title,
                    ClientName = t.id_user_clientNavigation.name,
                    ServiceName = t.id_serviceNavigation.name,
                    ServiceTypeName = t.id_serviceNavigation.id_service_typeNavigation.name,
                    Description = t.description,
                    Status = t.id_statusNavigation.name,
                    PostalCodeName = t.id_postal_codeNavigation.cod_auto_name + " / " +
                     t.id_postal_codeNavigation.cod_pro_name + " / " +
                     t.id_postal_codeNavigation.cod_loc_name,
                    IdPostalCode = t.id_postal_code,
                    ServiceId = t.id_service,
                    ServiceTypeId = t.id_serviceNavigation.id_service_type,
                    Active = t.active
                })
                .ToListAsync();

            return result;
        }
    }
}
