using Dima.Core.Handlers;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Events
{
    public partial class CreateEventPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public CreateEventRequest InputModel { get; set; } = new();

        #endregion


        #region Services

        [Inject]
        public IEventHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;

        #endregion


        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var result = await Handler.CreateAsync(InputModel);

                if (result.IsSuccess)
                {
                    NavigationManager.NavigateTo("/events");
                }
                else
                {
                    SnackBar.Add(result.Message, Severity.Success);
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

        //public void OnDateSelected(DateTime selectedDate)
        //{
        //    InputModel.StartDate = selectedDate;
        //}

        #endregion
    }
}
