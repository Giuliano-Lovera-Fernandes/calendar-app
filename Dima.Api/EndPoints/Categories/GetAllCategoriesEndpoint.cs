using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
           .WithName("Categories: Get All")
           .WithSummary("Recupera todas as categorias")
           .WithDescription("Recupera todaas as categorias")
           .WithOrder(5)
           .Produces<PagedResponse<List<Category?>>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler,
            [FromQuery]int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery]int pageSize = Configuration.DefaultPageSize
            )
        {
            var request = new GetAllCategoriesRequest
            {
                //UserId = "test@giuliano.io",
                UserId = user.Identity?.Name ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
            };


            var result = await handler.GetAllAsync(request);
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
