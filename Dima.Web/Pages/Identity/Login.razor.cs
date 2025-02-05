using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Security;

/*
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
 */

namespace Dima.Web.Pages.Identity
{
    public partial class LoginPage : ComponentBase
    {
        #region Services
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IAccountHandler Handler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        #endregion

        #region Properties
        public bool IsBusy { get; set; } = false;
        public LoginRequest InputModel { get; set; } = new();

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationAsync();
            var user = authState.User;

            //if (user.Identity is not null && user.Identity.IsAuthenticated)

            if (user.Identity is { IsAuthenticated: true })
            {
                await AuthenticationStateProvider.GetAuthenticationAsync();
                AuthenticationStateProvider.NotifyAuthenticateStateChanged();
                NavigationManager.NavigateTo("/");
            }

        }
        #endregion

        #region Methods
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var result = await Handler.LoginAsync(InputModel);

                if (result.IsSuccess)
                {
                    //Snackbar.Add(result.Message, Severity.Success);
                    await AuthenticationStateProvider.GetAuthenticationAsync();
                    AuthenticationStateProvider.NotifyAuthenticateStateChanged();
                    NavigationManager.NavigateTo("/");
                }

                else
                    Snackbar.Add(result.Message, Severity.Error);

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
