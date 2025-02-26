using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Events
{
    public partial class EditEventPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public UpdateEventRequest InputModel { get; set; } = new();

        #endregion

        #region Parameters

        [Parameter]
        public string Id { get; set; } = string.Empty;

        #endregion

        #region Services

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public IEventHandler Handler { get; set; } = null!;

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            GetEventByIdRequest? request = null;
            try
            {
                request = new GetEventByIdRequest
                {
                    Id = long.Parse(Id)
                };
            }
            catch (Exception ex)
            {
                Snackbar.Add("Parâmetro inválido", Severity.Error);
            }

            if (request is null)
                return;

            IsBusy = true;

            var response = await Handler.GetByIdAsync(request);

            try
            {
                if (response.IsSuccess || response.Data is not null)
                    InputModel = new UpdateEventRequest
                    {
                        Id = response.Data.Id,
                        Title = response.Data.Title,
                        Description = response.Data.Description ?? string.Empty,
                        EndDate = response.Data.EndDate,
                        IsActive = response.Data.IsActive
                    };
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
        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await Handler.UpdateAsync(InputModel);
                if (result.IsSuccess)
                {
                    Snackbar.Add("Evento atualizado", Severity.Success);
                    NavigationManager.NavigateTo("/eventos");
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
        #endregion

    }
}
