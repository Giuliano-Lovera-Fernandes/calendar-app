using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{    
    public class EventoHandler(AppDbContext context) : IEventHandler
    {
        public async Task<Response<Event>> CreateAsync(CreateEventRequest request)
        {
            try
            {
                var eventObj = new Event
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                    StartDate = DateTime.UtcNow,
                    EndDate = request.EndDate,
                    IsActive = true
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
                eventObj.EndDate = request.EndDate;
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
                var query = context.Events.
                AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

                var events = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query
                    .CountAsync();

                return new PagedResponse<List<Event>>(events, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Event?>>(null, 500, "[FP079] Não foi possível consultar os eventos");
            }
        }
    }
    
}
