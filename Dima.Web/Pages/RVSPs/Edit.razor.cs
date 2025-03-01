using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.RVSPs
{
    public partial class EditRVSPPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public UpdateRVSPRequest InputModel { get; set; } = new();
        public List<Event> Events { get; set; } = [];

        #endregion

        #region Parameter
        [Parameter]
        public string Id { get; set; } = string.Empty;
        #endregion

        #region Services

        [Inject]
        public IRVSPHandler RVSPHandler { get; set; } = null!;

        [Inject]
        public IEventHandler EventHandler { get; set; } = null!;        

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var result = await RVSPHandler.UpdateAsync(InputModel);

                if (result.IsSuccess && result.Data is not null)
                {
                    Snackbar.Add("Resposta ao evento atualizada com sucesso", Severity.Success);
                    NavigationManager.NavigateTo("/convites");
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
        #endregion

        #region Private Methods
        private async Task GetEventsAsync()
        {
            IsBusy = true;

            try
            {
                var request = new GetAllEventsRequest();
                var result = await EventHandler.GetAllAsync(request);
                if (result.IsSuccess || result.Data is not null)
                {
                    Events = result.Data ?? new List<Event>();
                    InputModel.EventId = Events.FirstOrDefault()?.Id ?? 0;
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

        private async Task GetRVSPByIdAsync()
        {
            IsBusy = true;

            try
            {
                var request = new GetRVSPByIdRequest { Id = long.Parse(Id) };
                var result = await RVSPHandler.GetByIdAsync(request);
                if (result.IsSuccess || result.Data is not null)
                {
                    

                    InputModel = new UpdateRVSPRequest
                    {
                        EventId = result.Data.EventId,
                        UserId = result.Data.UserId,
                        EventResponseDate = result.Data.EventResponseDate,                        
                        Id = result.Data.Id
                    };
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



        #region Overrides       

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            await GetRVSPByIdAsync();
            await GetEventsAsync();


        }
        #endregion
    }
}
