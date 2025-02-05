using Dima.Api.Common.Api;
using Dima.Api.EndPoints.Categories;
using Dima.Api.EndPoints.Identity;
using Dima.Api.EndPoints.Transactions;
using Dima.Api.Models;

namespace Dima.Api.EndPoints
{
    public static class EndPoint
    {
        public static void MapEndPoints(this WebApplication app)
        {
            var endpoints = app
                .MapGroup("");

            endpoints.MapGroup("/")
                .WithTags("Health Check")
                .MapGet("/", () => new { message = "Ok" });

            endpoints.MapGroup("v1/categories")
                .WithTags("Categories")
                .RequireAuthorization()
                .MapEndpoint<CreateCategoryEndPoint>(app)
                .MapEndpoint<UpdateCategoryEndpoint>(app)
                .MapEndpoint<DeleteCategoryEndpoint>(app)
                .MapEndpoint<GetCategoryByIdEndpoint>(app)
                .MapEndpoint<GetAllCategoriesEndpoint>(app);

            endpoints.MapGroup("v1/transactions")
                .WithTags("Transactions")
                .RequireAuthorization()
                .MapEndpoint<CreateTransactionEndpoint>(app)
                .MapEndpoint<UpdateTransactionEndpoint>(app)
                .MapEndpoint<DeleteTransactionEndpoint>(app)
                .MapEndpoint<GetTransactionByIdEndpoint>(app)
                .MapEndpoint<GetTransactionsByPeriodEndpoint>(app);

            //endpoints.MapGroup("v1/identity")
            //    .WithTags("Identity")
            //    .MapIdentityApi<User>();

            endpoints.MapGroup("v1/identity")
                .WithTags("Identity")                
                .MapEndpoint<LogoutEndpoint>(app)
                .MapEndpoint<GetRolesEndpoint>(app)
                .MapEndpoint<RegisterEndpoint>(app);




        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder routeBuilder, WebApplication app)
            where TEndpoint : IEndPoint
        {
            TEndpoint.Map(routeBuilder);
            return routeBuilder;
        }        
    }
}
