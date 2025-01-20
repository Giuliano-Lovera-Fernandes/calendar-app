using Dima.Api.Common.Api;
using Dima.Api.EndPoints.Categories;
using Dima.Api.EndPoints.Transactions;

namespace Dima.Api.EndPoints
{
    public static class EndPoint
    {
        public static void MapEndPoints(this WebApplication app)
        {
            var endpoints = app
                .MapGroup("v1");

            endpoints.MapGroup("categories")
                .WithTags("Categories")
                //.RequireAuthorization()
                .MapEndpoint<CreateCategoryEndPoint>(app)
                .MapEndpoint<UpdateCategoryEndpoint>(app)
                .MapEndpoint<DeleteCategoryEndpoint>(app)
                .MapEndpoint<GetCategoryByIdEndpoint>(app)
                .MapEndpoint<GetAllCategoriesEndpoint>(app);

            endpoints.MapGroup("transactions")
                .WithTags("Transactions")
            //.RequireAuthorization()
                .MapEndpoint<CreateTransactionEndpoint>(app)
                .MapEndpoint<UpdateTransactionEndpoint>(app)
                .MapEndpoint<DeleteTransactionEndpoint>(app)
                .MapEndpoint<GetTransactionByIdEndpoint>(app)
                .MapEndpoint<GetTransactionsByPeriodEndpoint>(app);
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder routeBuilder, WebApplication app)
            where TEndpoint : IEndPoint
        {
            TEndpoint.Map(routeBuilder);
            return routeBuilder;
        }
    }
}
