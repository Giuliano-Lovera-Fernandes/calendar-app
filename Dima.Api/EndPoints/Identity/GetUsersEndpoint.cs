using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.EndPoints.Identity
{
    //public class GetUsersEndpoint : IEndPoint
    //{
    //    public static void Map(IEndpointRouteBuilder routeBuilder)
    //    {
    //        routeBuilder.MapGet("/users", HandleAsync)
    //        .WithTags("Identity"); // Adiciona tags para Swagger
    //    }

    //    private static async Task<IResult> HandleAsync(UserManager<User> userManager)
    //    {
    //        var users = await userManager.Users
    //            .Select(u => new { u.Id, u.UserName, u.Email }) // Seleciona apenas os campos necessários
    //            .ToListAsync();

    //        return Results.Ok(users);
    //    }
    //}

    public class GetUsersEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/users", HandleAsync)
                .WithTags("Identity")
                .WithSummary("Recupera todos os usuários")
                .WithDescription("Retorna uma lista paginada de usuários")
                .Produces<PagedResponse<List<User>>>(); // Retorna um DTO ao invés da entidade
        }

        private static async Task<IResult> HandleAsync(
            UserManager<User> userManager,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = userManager.Users
                .OrderBy(u => u.UserName);

            var totalUsers = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new User // Usando DTO para evitar expor a entidade diretamente
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();

            var response = new PagedResponse<List<User>>(users, totalUsers, pageNumber, pageSize);

            return Results.Ok(response);
        }
    }
}
