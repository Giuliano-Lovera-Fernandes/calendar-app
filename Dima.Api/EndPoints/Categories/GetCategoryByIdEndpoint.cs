using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
           .WithName("Categories: Get By Id")
           .WithSummary("Recupera uma nova categoria")
           .WithDescription("Recupera uma nova categoria")
           .WithOrder(4)
           .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler,
            long id
            )
        {
            var request = new GetCategoryByIdRequest
            {
                UserId = user.Identity?.Name ?? string.Empty,
                Id = id
            };


            var result = await handler.GetByIdAsync(request);
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
