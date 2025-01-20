using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Transactions
{
    public class DeleteTransactionEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/{id}", HandleAsync)
           .WithName("Transactions: Delete")
           .WithSummary("Exclui uma nova transação")
           .WithDescription("Exclui uma nova transação")
           .WithOrder(3)
           .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            long id
            )
        {
            var request = new DeleteTransactionRequest
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
