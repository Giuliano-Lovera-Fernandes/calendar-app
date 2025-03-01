using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.RVSPs;
public partial class ListRVSPsPage : ComponentBase
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public List<RVSP> RVSPsList { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;

    #endregion


    #region Services
    [Inject]
    public IRVSPHandler Handler { get; set; } = null!;

    //[Inject]
    //public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar SnackBar { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    #endregion

    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllRVSPsRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                RVSPsList = result.Data ?? [];
            }
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    #region Methods

    public Func<RVSP, bool> Filter => rvsp =>
    {

        if (rvsp.UserId != null && rvsp.UserId.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    public async void OnDeleteButtonClickedAsync(long id)
    {
        var result = await DialogService.ShowMessageBox("ATENÇÃO",
            $"Se sua resposta ao convite não for excluída, significa que ela é a única associada ao evento. Caso contrário, todas as demais respostas serão removidas permanentemente. Esta é uma ação irreversível! " + $"Deseja Continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id)
    {
        try
        {
            await Handler.DeleteAsync(new DeleteRVSPRequest { Id = id });
            RVSPsList.RemoveAll(x => x.Id == id);
            SnackBar.Add($"Evento excluído com sucesso", Severity.Success);
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }
    }
    #endregion
}
