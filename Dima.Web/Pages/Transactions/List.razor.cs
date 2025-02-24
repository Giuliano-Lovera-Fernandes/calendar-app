using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace Dima.Web.Pages.Transactions
{
    public partial class ListTransactionsPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public List<Transaction> Transactions { get; set; } = [];
        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public int CurrentMonth { get; set; } = DateTime.Now.Month;

        public int[]  Years { get; set; }

        #endregion


        #region Services

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        [Inject]
        public ITransactionHandler Handler { get; set; } = null!;

        #endregion


        #region Private Methods

        private async Task GetTransactions()
        {
            IsBusy = true;
            try
            {
                var request = new GetTransactionsByPeriodRequest
                {
                    StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                    EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                    PageNumber = 1,
                    PageSize = 1000
                };

                var result = await Handler.GetByPeriodAsync(request);

                if (result.IsSuccess)
                    Transactions = result.Data ?? [];
            }
            catch(Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion


        #region Public Methods

        public Func<Transaction, bool> Filter => transaction =>
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return true;

            return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        };

        public async void OnDeleteButtonClickedAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("ATENÇÃO",
                $"Ao prosseguir o lançamento {title} será excluída. Esta é uma ação irreversível! " + $"Deseja Continuar?",
                yesText: "EXCLUIR",
                cancelText: "Cancelar");

            if (result is true)
                await OnDeleteAsync(id, title);

            StateHasChanged();
        }

        public async Task OnDeleteAsync(long id, string title)
        {
            IsBusy = true;

            try
            {
                var result = await Handler.DeleteAsync(new DeleteTransactionRequest { Id = id });

                if (result.IsSuccess)
                {
                    Snackbar.Add($"Lançamento {title} excluído", Severity.Success);
                    Transactions.RemoveAll(x => x.Id == id);
                }
                else
                {
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnSearchAsync()
        {
            await GetTransactions();
            StateHasChanged();
        }

        #endregion


        #region Overrides

        protected override async Task OnInitializedAsync() => 
            await GetTransactions();
       
        #endregion

    }
}
