using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Dima.Web.Handlers
{
    public class ReportHandler(IHttpClientFactory httpClientFactory) : IReportHandler
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

        public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesAsync(GetIncomesAndExpensesRequest request)
        {
            return await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>>>($"v1/reports/incomes-expenses")
                ?? new Response<List<IncomesAndExpenses>?>(null, 400, "Não foi possível obter os dados");
        }
        public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryAsync(GetExpensesByCategoryRequest request)
        {
            return await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>>>($"v1/reports/expenses")
                ?? new Response<List<ExpensesByCategory>?>(null, 400, "Não foi possível obter os dados");
        }

        public async Task<Response<FinancialSummary>?> GetFinancialSummaryAsync(GetFinancialSummaryRequest request)
        {
            return await _client.GetFromJsonAsync<Response<FinancialSummary?>>($"v1/reports/summary")
                ?? new Response<FinancialSummary?> (null, 400, "Não foi possível obter os dados");
        }


        public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryAsync(GetIncomesByCategoryRequest request)
        {
            var response = await _client.GetFromJsonAsync<Response<List<IncomesByCategory>>>($"v1/reports/incomes")
                ?? new Response<List<IncomesByCategory?>>(null, 400, "Não foi possível obter os dados");

            return response;
        }
    }
}
