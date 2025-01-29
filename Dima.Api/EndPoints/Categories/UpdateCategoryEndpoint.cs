using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories
{
    public class UpdateCategoryEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapPut("/{id}", HandleAsync)
           .WithName("Categories: Update")
           .WithSummary("Atualiza uma nova categoria")
           .WithDescription("Atualiza uma nova categoria")
           .WithOrder(2)
           .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler,
            UpdateCategoryRequest request,
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
