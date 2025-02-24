using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Events
{
    public class DeleteEventEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
           .WithName("Events: Delete")
           .WithSummary("Exclui um novo evento")
           .WithDescription("Exclui um novo evento")
           .WithOrder(3)
           .Produces<Response<Event?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IEventHandler handler,
            long id
            )
        {
            var request = new DeleteEventRequest
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
