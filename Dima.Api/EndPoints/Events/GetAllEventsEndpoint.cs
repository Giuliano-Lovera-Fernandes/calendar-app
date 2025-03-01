using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Events
{
    public class GetAllEventsEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
           .WithName("Events: Get All")
           .WithSummary("Recupera todos os eventos")
           .WithDescription("Recupera todos os eventos")
           .WithOrder(5)
           .Produces<PagedResponse<List<Event?>>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            IEventHandler handler,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize
            )
        {
            var request = new GetAllEventsRequest
            {
                //UserId = "test@giuliano.io",
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
