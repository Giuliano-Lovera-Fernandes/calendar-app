using Dima.Api.Common.Api;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.EndPoints.Identity
{
    public class GetUsersEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapGet("/users", HandleAsync)
            .WithTags("Identity"); // Adiciona tags para Swagger
        }

        private static async Task<IResult> HandleAsync(UserManager<User> userManager)
        {
            var users = await userManager.Users
                .Select(u => new { u.Id, u.UserName, u.Email }) // Seleciona apenas os campos necessários
                .ToListAsync();

            return Results.Ok(users);
        }
    }
}
