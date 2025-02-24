using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Events
{
    public class UpdateEventEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapPut("/{id}", HandleAsync)
           .WithName("Events: Update")
           .WithSummary("Atualiza uma novo evento")
           .WithDescription("Atualiza um novo evento")
           .WithOrder(2)
           .Produces<Response<Event?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IEventHandler handler,
            UpdateEventRequest request,
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
