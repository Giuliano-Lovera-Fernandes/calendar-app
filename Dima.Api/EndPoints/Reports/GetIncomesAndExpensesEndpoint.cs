using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Reports
{
    public class GetIncomesAndExpensesEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
           => routeBuilder.MapGet("/incomes-expenses", HandleAsync)
               .Produces<Response<List<IncomesAndExpenses>?>>();

        public static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,            
            IReportHandler handler)
        {
            var request = new GetIncomesAndExpensesRequest
            {
                UserId = user.Identity?.Name ?? string.Empty
            };

            //request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetIncomesAndExpensesAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
