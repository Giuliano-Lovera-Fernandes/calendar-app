using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.RVSPs
{
    public class UpdateRVSPEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapPut("/{id}", HandleAsync)
           .WithName("RVSPs: Update")
           .WithSummary("Atualiza uma nova resposta ao evento")
           .WithDescription("Atualiza uma nova resposta ao evento")
           .WithOrder(2)
           .Produces<Response<RVSP?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IRVSPHandler handler,
            UpdateRVSPRequest request,
            long id
            )
        {
            //request.UserId = "test@giuliano.io";
            request.UserId = user.Identity?.Name ?? string.Empty;
            request.Id = id;
            var result = await handler.UpdateAsync(request);
            //if (result.IsSuccess)
            //{
            //    return TypedResults.Created($"/{result.Data.Id}", result.Data);
            //}

            //return TypedResults.BadRequest(result.Data);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
