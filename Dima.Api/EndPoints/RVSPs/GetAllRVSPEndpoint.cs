using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.EndPoints.RVSPs
{
    public class GetAllRVSPsEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
           .WithName("RVSPs: Get All")
           .WithSummary("Recupera todas as repostas aos eventos")
           .WithDescription("Recupera todas as repostas aos eventos")
           .WithOrder(5)
           .Produces<PagedResponse<List<RVSP?>>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IRVSPHandler handler,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize
            )
        {
            var request = new GetAllRVSPsRequest
            {                
                UserId = user.Identity?.Name ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await handler.GetAllAsync(request);            

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
