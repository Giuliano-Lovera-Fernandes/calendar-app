using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.EndPoints.Identity
{
    public class RegisterEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapPost("/v1/identity/register", HandleAsync)
                .WithTags("Identity"); // Você pode adicionar tags para o Swagger aqui
                
        }

        private static async Task<IResult> HandleAsync(
        RegisterRequest request,
        UserManager<User> userManager // Usando diretamente o UserManager
    )
        {
            // Verifica se o e-mail já está registrado
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Results.BadRequest(new { message = "Já existe um usuário com esse e-mail." });
            }

            // Cria um novo usuário
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email
            };

            // Cria o usuário no banco de dados com a senha fornecida
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Results.Ok(new { message = "Cadastro realizado com sucesso!" });
            }

            return Results.BadRequest(new { message = "Não foi possível criar o usuário." });
        }
    }
}
