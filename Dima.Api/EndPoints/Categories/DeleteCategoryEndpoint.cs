using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Categories
{
    public class DeleteCategoryEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
           .WithName("Categories: Delete")
           .WithSummary("Exclui uma nova categoria")
           .WithDescription("Exclui uma nova categoria")
           .WithOrder(3)
           .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,            
            long id
            )
        {
            var request = new DeleteCategoryRequest
            {
                UserId = "test@giuliano.io",
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
