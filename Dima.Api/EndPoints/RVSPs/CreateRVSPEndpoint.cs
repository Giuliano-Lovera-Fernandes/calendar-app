using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.RVSPs
{
    public class CreateRVSPEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("RVSPs: Create")
            .WithSummary("Cria uma nova resposta ao evento")
            .WithDescription("Cria uma nova resposta ao evento")
            .WithOrder(1)
            .Produces<Response<RVSP?>>();

        public static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IRVSPHandler handler,
            CreateRVSPRequest request)
        {
            //Caso futuramente seja retomado o processo:
            //request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.CreateAsync(request);           

            return Results.Json(result, statusCode: result.Code);
        }
    }
}
