using Microsoft.AspNetCore.Components;
using TimeKeep.Client.Services;
using TimeKeep.Shared;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Client.Pages.User
{
    public partial class Register
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private RegisterVM RegisterVM { get; set; } = new RegisterVM();
        private AuthResponse ServerResponse { get; set; }
        private bool ShowServerErrors { get; set; }
        private bool DisableSubmit { get; set; } = false;

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;
            ServerResponse = await UsersService.Register(RegisterVM);

            if (ServerResponse.IsSuccess)
            {
                Navigation.NavigateTo("/");
            }
            else
            {
                ShowServerErrors = true;
                DisableSubmit = false;
            }
        }
    }
}
