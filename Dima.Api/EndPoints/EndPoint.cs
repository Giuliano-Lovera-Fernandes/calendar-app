using Dima.Api.Common.Api;
using Dima.Api.EndPoints.Categories;
using Dima.Api.EndPoints.Events;
using Dima.Api.EndPoints.Identity;
using Dima.Api.EndPoints.Reports;
using Dima.Api.EndPoints.Transactions;
using Dima.Api.Models;
using Dima.Core.Requests.Reports;

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


            endpoints.MapGroup("v1/reports")
                .WithTags("Reports")
                .RequireAuthorization()
                .MapEndpoint<GetIncomesAndExpensesEndpoint>(app)
                .MapEndpoint<GetIncomesByCategoryEndpoint>(app)
                .MapEndpoint<GetFinancialSummaryEndpoint>(app)
                .MapEndpoint<GetExpensesByCategoryEndpoint>(app);

            endpoints.MapGroup("v1/events")
                .WithTags("Events")
                .RequireAuthorization()
                .MapEndpoint<CreateEventEndpoint>(app)
                .MapEndpoint<UpdateEventEndpoint>(app)
                .MapEndpoint<DeleteEventEndpoint>(app)
                //.MapEndpoint<GetCategoryByIdEndpoint>(app)
                .MapEndpoint<GetAllEventsEndpoint>(app);

        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder routeBuilder, WebApplication app)
            where TEndpoint : IEndPoint
        {
            TEndpoint.Map(routeBuilder);
            return routeBuilder;
        }        
    }
}
