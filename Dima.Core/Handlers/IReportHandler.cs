using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Handlers
{
    public interface IReportHandler
    {
        Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesAsync(GetIncomesAndExpensesRequest request);
        Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryAsync(GetIncomesByCategoryRequest request);
        Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryAsync(GetExpensesByCategoryRequest request);
        Task<Response<FinancialSummary>?> GetFinancialSummaryAsync(GetFinancialSummaryRequest request);
    }
}
