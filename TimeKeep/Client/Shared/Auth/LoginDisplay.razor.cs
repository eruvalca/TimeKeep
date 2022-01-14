using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Shared.Auth
{
    public partial class LoginDisplay
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private UsersService UsersService { get; set; }

        private TimeKeepUser User { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var isAuthenticated = (await AuthenticationState).User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                User = await UsersService.GetUserDetails();
            }

            StateHasChanged();
            await base.OnParametersSetAsync();
        }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await UsersService.Logout();
            Navigation.NavigateTo("/");
        }
    }
}
