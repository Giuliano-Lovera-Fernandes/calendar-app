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
            //request.UserId = "test@giuliano.io";
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.CreateAsync(request);
            //if (result.IsSuccess)
            //{
            //    return TypedResults.Created($"/{result.Data.Id}", result.Data);
            //}

            //return TypedResults.BadRequest(result.Data);

            return result.IsSuccess
                ? TypedResults.Created($"/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result.Data); ;
        }
    }
}
