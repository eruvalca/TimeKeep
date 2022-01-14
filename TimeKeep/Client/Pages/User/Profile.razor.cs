using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeep.Client.Services;
using TimeKeep.Shared;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Client.Pages.User
{
    [Authorize]
    public partial class Profile
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private PTOEntriesService PTOEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private TimeKeepUser TimeKeepUser { get; set; }
        private UserVM User { get; set; }
        private AuthResponse ServerResponse { get; set; }
        private bool ShowServerErrors { get; set; }
        private bool DisableSubmit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            TimeKeepUser = await UsersService.GetUserDetails();
            var vacationCarriedOverEntry = await PTOEntriesService.GetVacationCarriedOverEntryByUserId(TimeKeepUser.Id);

            var vacationDaysCarriedOver = vacationCarriedOverEntry is null ? 0 : vacationCarriedOverEntry.PTOHours / 8;
            
            User = new UserVM
            {
                Id = TimeKeepUser.Id,
                FirstName = TimeKeepUser.FirstName,
                LastName = TimeKeepUser.LastName,
                HireDate = TimeKeepUser.HireDate,
                VacationDaysAccruedPerMonth = TimeKeepUser.VacationDaysAccruedPerMonth,
                SickHoursAccruedPerMonth = TimeKeepUser.SickHoursAccruedPerMonth,
                PersonalDaysPerYear = TimeKeepUser.PersonalDaysPerYear,
                VacationDaysCarriedOver = vacationDaysCarriedOver
            };
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;
            ServerResponse = await UsersService.UpdateUser(User);

            if (ServerResponse.IsSuccess)
            {
                await UsersService.Logout();
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
