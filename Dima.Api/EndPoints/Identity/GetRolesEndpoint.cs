using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Identity
{
    public class GetRolesEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
        => routeBuilder.MapGet("/roles", HandleAsync)
            .RequireAuthorization();
        //.WithName("Transactions: Update")
        //.WithSummary("Atualiza uma nova transação")
        //.WithDescription("Atualiza uma nova transação")
        //.WithOrder(2)
        //.Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user
            ) 
        {
            if (user.Identity is null || !user.Identity.IsAuthenticated)
                           return Results.Unauthorized();

            var identity = (ClaimsIdentity)user.Identity;
            var roles = identity
            .FindAll(identity.RoleClaimType)
            //.Select(c => new
            //{
            //    c.Issuer,
            //    c.OriginalIssuer,
            //    c.Type,
            //    c.Value,
            //    c.ValueType
            //});
            .Select(c => new RoleClaim
            {
                Issuer = c.Issuer,
                OriginalIssuer = c.OriginalIssuer,
                Type = c.Type,
                Value = c.Value,
                ValueType = c.ValueType
            });


            return TypedResults.Json(roles);
        }
    }
}
