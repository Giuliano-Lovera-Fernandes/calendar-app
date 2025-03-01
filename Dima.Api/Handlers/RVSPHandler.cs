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
                if (string.IsNullOrWhiteSpace(request.UserId))
                {
                    return new Response<RVSP?>(null, 409, "A resposta do convite ao evento precisa um email de usuário.");
                }

                if (request.EventId == 0)
                {
                    return new Response<RVSP?>(null, 409, "É necessário adicionar um evento.");
                }

                if (request.EventResponseDate < DateTime.Now.Date)
                {
                    return new Response<RVSP?>(null, 409, "A data da resposta ao evento deve ser no futuro.");
                }

                var rvsp = new RVSP
                {
                    UserId = request.UserId,
                    EventResponseDate = request.EventResponseDate ?? DateTime.UtcNow,
                    EventId = request.EventId,                    
                };

                await context.RVSPs.AddAsync(rvsp);
                await context.SaveChangesAsync();

                return new Response<RVSP?>(rvsp, 201, "Resposta para convite enviada com sucesso");
            }
            catch (Exception ex)
            {                
                return new Response<RVSP?>(null, 500, "Não foi possível criar a resposta do Evento");
            }
        }

        public async Task<Response<RVSP?>> UpdateAsync(UpdateRVSPRequest request)
        {
            try
            {
                //var rvsp = await context.RVSPs.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                var rvsp = await context.RVSPs.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (rvsp == null)
                {
                    return new Response<RVSP?>(null, 404, "Resposta ao evento não encontrada");
                }              


                rvsp.UserId = request.UserId;
                rvsp.EventResponseStatus = request.EventResponseStatus;
                rvsp.EventResponseDate = request.EventResponseDate ?? DateTime.Now;

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

        public async Task<PagedResponse<List<RVSP>>> GetAllAsync(GetAllRVSPsRequest request)
        {
            try
            {
                var query = context.RVSPs
                .AsNoTracking()
                //.Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.EventResponseDate);

                var rvsps = await query
                    //Para ajustes futuros, pode ser interessante
                    //.Include(r => r.Event)
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

        public async Task<Response<RVSP?>> GetByIdAsync(GetRVSPByIdRequest request)
        {
            try
            {
                var rvsp = await context.RVSPs
                    .AsNoTracking()
                    //.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                return rvsp is null
                    ? new Response<RVSP?>(null, 404, "Evento não encontrada")
                    : new Response<RVSP?>(rvsp);
            }
            catch
            {
                return new Response<RVSP?>(null, 500, "[FP079] Não foi possível encontrar o evento");
            }
        }
    }    
}
