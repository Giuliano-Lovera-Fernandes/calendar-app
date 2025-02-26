using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Dima.Api.Handlers
{    
    public class EventoHandler(AppDbContext context) : IEventHandler
    {
        public async Task<Response<Event>> CreateAsync(CreateEventRequest request)
        {
            //request.StartDate = DateTime.UtcNow;

            try
            {
                // Validar se o horário de término é posterior ao horário de início
                //assegura que o evento não será criado com um horário de término inválido
                if (request.EndDate <= DateTime.UtcNow)
                {
                    //return new Response<Event?>(null, 201, "A data de término não pode ser anterior ou igual à data atual.");
                    
                    return new Response<Event>(null, 422, "A data de início não pode ser posterior à data de término.");
                  

                    //return BadRequest(new Response<Event?>(null, 400, "A data de início não pode ser posterior à data de término."));


                }

                var exception = new Exception();
                if (request.StartDate >= request.EndDate)
                {
                    //throw new Exception("A data de início não pode ser posterior à data de término.");
                    return new Response<Event?>(null, 422, "A data de início não pode ser posterior à data de término.");
                }

                // Verificar se já existe um evento no mesmo intervalo de tempo
                var overlappingEvent = await context.Events
                    .Where(e => e.StartDate < request.EndDate && e.EndDate > request.StartDate)
                    .AnyAsync();

                if (overlappingEvent)
                {
                    return new Response<Event?>(null, 201, "Já existe um evento no mesmo horário. Por favor, escolha outro horário.");
                }

                bool isMultiDayEvent = false;
                // Verificar se o evento dura mais de um dia
                if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                    var duration = request.EndDate.Value - request.StartDate.Value;
                    isMultiDayEvent = duration.TotalDays > 1;
                }

                var eventObj = new Event
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                    StartDate = DateTime.UtcNow,
                    EndDate = request.EndDate ?? DateTime.UtcNow,
                    IsActive = true,
                    IsMultiDayEvent = isMultiDayEvent
                };

                await context.Events.AddAsync(eventObj);
                await context.SaveChangesAsync();

                return new Response<Event?>(eventObj, 201, "Evento criada com sucesso");
            }
            catch (Exception ex)
            {
                // Serilog
                //Console.WriteLine(ex);
                //throw new Exception(message: "Falha ao criar Categoria");
                return new Response<Event?>(null, 500, "Não foi possível criar o Evento");
            }
        }

        public async Task<Response<Event?>> UpdateAsync(UpdateEventRequest request)
        {
            try
            {
                var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (eventObj == null)
                {
                    return new Response<Event?>(null, 404, "Evento não encontrado");
                }
                eventObj.Title = request.Title;
                eventObj.Description = request.Description;
                eventObj.EndDate = request.EndDate ?? DateTime.UtcNow;
                eventObj.IsActive = request.IsActive;

                context.Events.Update(eventObj);
                await context.SaveChangesAsync();

                return new Response<Event?>(eventObj, message: "Evento atualizado com sucesso");
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
                var eventObj = await context.Events.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
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

        //public async Task<Response<Category>> GetByIdAsync(GetCategoryByIdRequest request)
        //{
        //    try
        //    {
        //        var category = await context.Categories
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
        //        return category is null
        //            ? new Response<Category?>(null, 404, "Categoria não encontrada")
        //            : new Response<Category?>(category);
        //    }
        //    catch
        //    {
        //        return new Response<Category?>(null, 500, "[FP079] Não foi possível excluir a Categoria");
        //    }
        //}

        public async Task<PagedResponse<List<Event>>> GetAllAsync(GetAllEventsRequest request)
        {
            try
            {
                var query = context.Events
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

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
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                return eventObj is null
                    ? new Response<Event?>(null, 404, "Evento não encontrada")
                    : new Response<Event?>(eventObj);
            }
            catch
            {
                return new Response<Event?>(null, 500, "[FP079] Não foi possível encontrar o evento");
            }
        }
    }
    
}
