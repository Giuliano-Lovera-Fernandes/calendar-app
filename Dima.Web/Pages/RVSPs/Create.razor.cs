using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Models.Account;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Events;
using Dima.Core.Requests.RVSPs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.RVSPs
{
    public partial class CreateRVSPPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public CreateRVSPRequest InputModel { get; set; } = new();
        public List<Event> Eventos { get; set; } = [];
        public List<User> Users { get; set; } = [];

        #endregion

        #region Services

        [Inject]
        public IRVSPHandler RVSPHandler { get; set; } = null!;

        [Inject]
        public IEventHandler EventHandler { get; set; } = null!;

        [Inject]
        public IAccountHandler AccountHandler { get; set; } = null!;

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
                var result = await RVSPHandler.CreateAsync(InputModel);

                if (result.IsSuccess && result.Data is not null)
                {
                    Snackbar.Add(result.Message, Severity.Success);
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

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            try
            {
                var requestEvents = new GetAllEventsRequest();
                var request = new GetAllUsersRequest();
                var resultEvents = await EventHandler.GetAllAsync(requestEvents);
                var result = await AccountHandler.GetAllUsersAsync(request);
                if (result.IsSuccess && resultEvents.IsSuccess)
                {
                    Users = result.Data ?? new List<User>();
                    Eventos = resultEvents.Data ?? new List<Event>();                   
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
