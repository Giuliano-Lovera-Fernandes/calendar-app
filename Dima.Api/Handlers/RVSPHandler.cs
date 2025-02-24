using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    
    public class RVSPHandler(AppDbContext context) : IRVSPHandler
    {
        public async Task<Response<RVSP>> CreateAsync(CreateRVSPRequest request)
        {
            try
            {
                var rvsp = new RVSP
                {
                    UserId = request.UserId,
                    EventResponseDate = request.EventResponseDate,
                    EventId = request.EventId,                    
                };

                await context.RVSPs.AddAsync(rvsp);
                await context.SaveChangesAsync();

                return new Response<RVSP?>(rvsp, 201, "Resposta do evento criada com sucesso");
            }
            catch (Exception ex)
            {
                // Serilog
                //Console.WriteLine(ex);
                //throw new Exception(message: "Falha ao criar Categoria");
                return new Response<RVSP?>(null, 500, "Não foi possível criar a resposta do Evento");
            }
        }

        public async Task<Response<RVSP?>> UpdateAsync(UpdateRVSPRequest request)
        {
            try
            {
                var rvsp = await context.RVSPs.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (rvsp == null)
                {
                    return new Response<RVSP?>(null, 404, "Resposta ao evento não encontrada");
                }
                rvsp.EventResponseStatus = request.EventResponseStatus;                                

                context.RVSPs.Update(rvsp);
                await context.SaveChangesAsync();

                return new Response<RVSP?>(rvsp, message: "Resposta ao evento atualizada com sucesso");
            }
            catch
            {
                return new Response<RVSP?>(null, 500, "[FP079] Não foi possível alterar a resposta do Evento");
            }

        }

        public async Task<Response<RVSP>> DeleteAsync(DeleteRVSPRequest request)
        {
            try
            {
                var rvsp = await context.RVSPs.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (rvsp == null)
                {
                    return new Response<RVSP?>(null, 404, "Resposta ao evento não encontrada");
                }

                context.RVSPs.Remove(rvsp);
                await context.SaveChangesAsync();

                return new Response<RVSP?>(rvsp, 200, "Resposta ao evento excluída com sucesso");
            }
            catch
            {
                return new Response<RVSP?>(null, 500, "[FP079] Não foi possível excluir a resposta ao evento");
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

        public async Task<PagedResponse<List<RVSP>>> GetAllAsync(GetAllRVSPsRequest request)
        {
            try
            {
                var query = context.RVSPs.
                AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.EventResponseDate);

                var rvsps = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query
                    .CountAsync();

                return new PagedResponse<List<RVSP>>(rvsps, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<RVSP?>>(null, 500, "[FP079] Não foi possível consultar as resposta dos eventos");
            }
        }
    }
    
}
