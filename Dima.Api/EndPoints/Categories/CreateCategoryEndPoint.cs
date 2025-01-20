using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;

namespace Dima.Api.EndPoints.Categories
{
    public class CreateCategoryEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Cria uma nova categoria")
            .WithDescription("Cria uma nova categoria")
            .WithOrder(1)
            .Produces<Response<Category?>>(); 

        public static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
        {
            request.UserId = "test@giuliano.io";
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
