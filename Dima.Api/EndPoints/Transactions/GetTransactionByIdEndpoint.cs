using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Transactions
{
    public class GetTransactionByIdEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
           .WithName("Transactions: Get By Id")
           .WithSummary("Recupera uma nova transação")
           .WithDescription("Recupera uma nova transação")
           .WithOrder(4)
           .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ITransactionHandler handler,
            long id
            )
        {
            var request = new GetTransactionByIdRequest
            {
                //UserId = "test@giuliano.io",
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
