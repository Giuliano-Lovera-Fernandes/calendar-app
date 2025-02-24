using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.EndPoints.Reports
{
    public class GetFinancialSummaryEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder routeBuilder)
            => routeBuilder.MapGet("/summary", HandleAsync)
                .Produces<Response<FinancialSummary>?>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,           
            IReportHandler handler)
        {
            var request = new GetFinancialSummaryRequest
            {
                UserId = user.Identity?.Name ?? string.Empty
            };

            //request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetFinancialSummaryAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
