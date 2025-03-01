using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.RVSPs
{
    public class GetRVSPByIdEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
           .WithName("RVSPs: Get By Id")
           .WithSummary("Recupera uma nova resposta ao evento")
           .WithDescription("Recupera uma nova resposta ao evento")
           .WithOrder(4)
           .Produces<Response<RVSP?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IRVSPHandler handler,
            long id
            )
        {
            var request = new GetRVSPByIdRequest
            {
                UserId = user.Identity?.Name ?? string.Empty,
                Id = id
            };

            var result = await handler.GetByIdAsync(request);            

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
