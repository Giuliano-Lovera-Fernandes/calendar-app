using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Events
{
    public class CreateEventEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Events: Create")
            .WithSummary("Cria um novo evento")
            .WithDescription("Cria um novo evento")
            .WithOrder(1)
            .Produces<Response<Event?>>();

        public static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IEventHandler handler,
            CreateEventRequest request)
        {           
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.CreateAsync(request);            

            return Results.Json(result, statusCode: result.Code);
        }
    }
}
