using Dima.Core.Handlers;
using Dima.Core.Requests.Events;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Dima.Web.Pages.Events
{
    public partial class CreateEventPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public CreateEventRequest InputModel { get; set; } = new();

        [Required(ErrorMessage = "A data é obrigatória!")]
        public DateTime? StartDate { get; set; }

        #endregion


        #region Services

        [Inject]
        public IEventHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ISnackbar SnackBar { get; set; } = null!;

        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        #endregion


        #region Methods

        public async Task OnValidSubmitAsync()
        {
            if (InputModel.StartDate == null)
            {
                var result = await DialogService.ShowMessageBox("ATENÇÃO",
                $"A {InputModel.StartDate} precisa ser preenchida! ",
                cancelText: "Cancelar");
            }

            IsBusy = true;
            try
            {
                var result = await Handler.CreateAsync(InputModel);

                if (result.IsSuccess && result.Data is not null)
                {
                    SnackBar.Add("Evento criado com sucesso", Severity.Success);
                    NavigationManager.NavigateTo("/eventos");
                }
                else
                {
                    SnackBar.Add(result.Message, Severity.Error);
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
    }
}
