using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Transactions
{
    public class CreateTransactionEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapPost("/", HandleAsync)
           .WithName("Transactions: Create")
           .WithSummary("Cria uma nova transação")
           .WithDescription("Cria uma nova transação")
           .WithOrder(1)
           .Produces<Response<Transaction?>>();

        public static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            CreateTransactionRequest request)
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
