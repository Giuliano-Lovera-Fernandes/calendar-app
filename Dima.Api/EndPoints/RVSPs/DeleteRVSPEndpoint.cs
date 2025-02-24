using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.RVSPs
{
    public class DeleteRVSPEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
           .WithName("RVSPs: Delete")
           .WithSummary("Exclui uma nova resposta ao evento")
           .WithDescription("Exclui uma nova resposta ao evento")
           .WithOrder(3)
           .Produces<Response<RVSP?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IRVSPHandler handler,
            long id
            )
        {
            var request = new DeleteRVSPRequest
            {
                //UserId = "test@giuliano.io",
                UserId = user.Identity?.Name ?? string.Empty,
                Id = id
            };


            var result = await handler.DeleteAsync(request);
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
