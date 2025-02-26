using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Events;

public partial class ListEventsPage : ComponentBase
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public List<Event> Events { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;

    #endregion


    #region Services
    [Inject]
    public IEventHandler Handler { get; set; } = null!;

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
            var request = new GetAllEventsRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                Events = result.Data ?? [];
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

    public Func<Event, bool> Filter => eventObj =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;

        if (eventObj.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (eventObj.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        if (eventObj.Description is not null && eventObj.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox("ATENÇÃO",
            $"Ao prosseguir a categoria {title} será excluída. Esta é uma ação irreversível! " + $"Deseja Continuar?",
            yesText: "EXCLUIR",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            await Handler.DeleteAsync(new DeleteEventRequest { Id = id });
            Events.RemoveAll(x => x.Id == id);
            SnackBar.Add($"Evento {title} excluído", Severity.Success);
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }
    }
    #endregion

}
