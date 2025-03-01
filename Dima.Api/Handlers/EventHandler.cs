using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.SqlServer.Server;
using System;

namespace Dima.Api.Handlers
{    
    public class EventoHandler(AppDbContext context) : IEventHandler
    {
        public async Task<Response<Event>> CreateAsync(CreateEventRequest request)
        {
            //var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);


            if (!request.StartDate.HasValue || !request.EndDate.HasValue)
            {
                return new Response<Event>(null, 422, "A data de início e a data de término são obrigatórias.");
            }            

            TimeSpan startTime = request.StartTime ?? TimeSpan.Zero;
            TimeSpan endTime = request.EndTime ?? TimeSpan.Zero;

            DateTime startDateTime = request.StartDate.Value.Date.Add(startTime);
            DateTime endDateTime = request.EndDate.Value.Date.Add(endTime);           

            try
            {
                var validacao = new Response<Event>();

                validacao = await ValidarEventoAsync(startDateTime, endDateTime);

                if (validacao.Code == 422)
                {
                    return validacao; 
                }

                var existingEvent = await context.Events.FirstOrDefaultAsync(x => x.Title.ToLower() == request.Title.ToLower());
                if (existingEvent != null)
                {
                    return new Response<Event>(null, 422, "Já existe um evento com esse título.");
                }

                bool isMultiDayEvent = IsMultiDayEvent(startDateTime, endDateTime);




                var eventObj = new Event
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    IsActive = true,
                    IsMultiDayEvent = isMultiDayEvent
                };

                await context.Events.AddAsync(eventObj);
                await context.SaveChangesAsync();

                return new Response<Event?>(eventObj, 201, "Evento criado com sucesso");
            }
            catch (Exception ex)
            {                
                return new Response<Event?>(null, 500, "Não foi possível criar o Evento");
            }
        }

        public async Task<Response<Event?>> UpdateAsync(UpdateEventRequest request)
        {
            try
            {
                //var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
               
                if (eventObj == null)
                {
                    return new Response<Event?>(null, 404, "Evento não encontrado");
                }

                TimeSpan startTime = request.StartTime ?? TimeSpan.Zero;
                TimeSpan endTime = request.EndTime ?? TimeSpan.Zero;

                DateTime startDateTime = request.StartDate.Value.Date.Add(startTime);
                DateTime endDateTime = request.EndDate.Value.Date.Add(endTime);

                bool hasDateChanged = eventObj.StartDate != startDateTime || eventObj.EndDate != endDateTime;

                var validacao = new Response<Event>();
                
                if (hasDateChanged)
                {
                    validacao = await ValidarEventoAsync(startDateTime, endDateTime);
                }               
                
                if (validacao.Code == 422)
                {
                    validacao.Data = null;
                    return validacao;
                }

                bool isMultiDayEvent = IsMultiDayEvent(startDateTime, endDateTime);
                
                

                if (request.StartDate == null || request.EndDate == null)
                {
                    return new Response<Event?>(null, 422, "As datas de início e término são obrigatórias.");
                }
                eventObj.Title = request.Title;
                eventObj.Description = request.Description;
                eventObj.StartDate = startDateTime;
                eventObj.EndDate = endDateTime;
                eventObj.IsActive = request.IsActive;
                eventObj.IsMultiDayEvent = isMultiDayEvent;

                context.Events.Update(eventObj);
                await context.SaveChangesAsync();

                return new Response<Event?>(eventObj, 201, message: "Evento atualizado com sucesso");
            }
            catch
            {
                return new Response<Event?>(null, 500, "[FP079] Não foi possível alterar o Evento");
            }
        }

        public async Task<Response<Event>> DeleteAsync(DeleteEventRequest request)
        {
            try
            {
                var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (eventObj == null)
                {
                    return new Response<Event?>(null, 404, "Evento não encontrado");
                }

                context.Events.Remove(eventObj);
                await context.SaveChangesAsync();

                return new Response<Event?>(eventObj, 200, "Evento excluído com sucesso");
            }
            catch
            {
                return new Response<Event?>(null, 500, "[FP079] Não foi possível excluir o evento");
            }
        }        

        public async Task<PagedResponse<List<Event>>> GetAllAsync(GetAllEventsRequest request)
        {
            try
            {
                var query = context.Events
                .AsNoTracking()
                //.Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.StartDate);

                var events = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Carregar todos os RSVP's relacionados aos eventos carregados
                var eventIds = events.Select(e => e.Id).ToList();
                var rsvps = await context.RVSPs
                .Where(r => eventIds.Contains(r.EventId))
                .ToListAsync();

                //Associar os RSVP's aos eventos
                foreach (var eventObj in events)
                {
                    eventObj.RSVPs = rsvps
                        .Where(r => r.EventId == eventObj.Id)
                        .ToList();
                }

                var count = await query
                    .CountAsync();


                return new PagedResponse<List<Event>>(events, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Event?>>(null, 500, "[FP079] Não foi possível consultar os eventos");
            }
        }

        public async Task<Response<Event?>> GetByIdAsync(GetEventByIdRequest request)
        {
            try
           {
                var eventObj = await context.Events
                    .AsNoTracking()                    
                    //.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                return eventObj is null
                    ? new Response<Event?>(null, 404, "Evento não encontrada")
                    : new Response<Event?>(eventObj);
            }
            catch
            {
                return new Response<Event?>(null, 500, "[FP079] Não foi possível encontrar o evento");
            }
        }        

        private bool IsMultiDayEvent(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
                return false; // Se uma das datas for nula, não pode ser um evento de múltiplos dias

            return (endDate.Value.Date - startDate.Value.Date).TotalDays >= 1;
        }

        private async Task<Response<Event?>> ValidarEventoAsync(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                return new Response<Event>(null, 422, "A data de início do evento não pode ser nula.");
            }

            if (endDate == null)
            {
                return new Response<Event>(null, 422, "A data do fim do evento não pode ser nula.");
            }

            if (startDate > endDate)
            {
                return new Response<Event?>(null, 422, "A hora ou data de início não pode ser posterior à hora ou data de término.");
            }

            if (startDate < DateTime.Now)
            {
                return new Response<Event?>(null, 422, "A hora ou data de início deve ser no futuro.");
            }

            if (endDate < DateTime.Now)
            {
                return new Response<Event?>(null, 422, "A hora ou data de término deve ser no futuro.");
            }            

            //Utilizado para verificação:
            //var overlappingEvents = await context.Events
            //       .Where(e => e.StartDate <= endDate.Value &&
            //        e.EndDate >= startDate.Value)
            //    .AnyAsync();

            var overlappingEvent = await context.Events
                .Where(e => e.StartDate < endDate.Value.AddMinutes(1) &&
                            e.EndDate > startDate.Value.AddMinutes(-1))
                .AnyAsync();

            var allEvents = await context.Events.ToListAsync();

            if (overlappingEvent)
            {
                return new Response<Event?>(null, 422, "Já existe um evento no mesmo horário ou data. Por favor, escolha outro horário ou data.");
            }

            return new Response<Event?>(null, 200, "Validação concluída com sucesso.");
        }
    }
}
