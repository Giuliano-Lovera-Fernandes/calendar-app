using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using System.Data;
using System.Globalization;

namespace Dima.Web.Components.Reports
{
    public  partial class ExpensesByCategoryChartComponent : ComponentBase
    {
        #region Propeties
        public List<double> Data { get; set; } = [];
        public List<string> Labels { get; set; } = [];
        #endregion

        #region Services
        [Inject]
        public IReportHandler Handler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            await GetExpensesByCategoryAsync();
        }
        

        private async Task GetExpensesByCategoryAsync()
        {
            var request = new GetExpensesByCategoryRequest();
            var result = await Handler.GetExpensesByCategoryAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Falha ao obter dados do relatório", Severity.Error);
                return;
            }

            foreach (var item in result.Data)
            {
                //Labels.Add($"{item.Category}({item.Expenses: C})");
                Labels.Add($"{item.Category} ({item.Expenses.ToString("C", new CultureInfo("pt-BR"))})");
                Data.Add(-(double)item.Expenses);
            }
        }

        #endregion region
    }
}
