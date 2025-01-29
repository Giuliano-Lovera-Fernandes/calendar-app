using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Identity
{
    public class LogoutEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
        => routeBuilder.MapPost("/logout", HandleAsync)
            .RequireAuthorization();
           //.WithName("Transactions: Update")
           //.WithSummary("Atualiza uma nova transação")
           //.WithDescription("Atualiza uma nova transação")
           //.WithOrder(2)
           //.Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            SignInManager<User> signInManager
            )
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
    }
}
