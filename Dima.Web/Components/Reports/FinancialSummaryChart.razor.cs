using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace Dima.Web.Components.Reports
{
    public partial class FinancialSummaryChartComponent : ComponentBase
    {
        #region Properties
        public bool ShowValues { get; set; } = true;
        public FinancialSummary? Summary  { get; set; }
        #endregion

        #region Services
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IReportHandler Handler { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task  OnInitializedAsync()
        {
            var request = new GetFinancialSummaryRequest();
            var result = await Handler.GetFinancialSummaryAsync(request);

            if (result.IsSuccess)
                Summary = result.Data;
        }
        #endregion

        #region Methods

        public void ToggleShowValues()
            => ShowValues = !ShowValues;
        #endregion
    }
}
