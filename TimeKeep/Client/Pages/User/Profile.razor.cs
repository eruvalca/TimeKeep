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
        private decimal _vacationCarriedOver { get; set; }
        private decimal VacationCarriedOver
        {
            get { return _vacationCarriedOver; }
            set
            {
                _vacationCarriedOver = value;

                if (_vacationCarriedOver > 5)
                {
                    _vacationCarriedOver = 5;
                }

                if (_vacationCarriedOver < 0)
                {
                    _vacationCarriedOver = 0;
                }
            }
        }
        private bool ShowServerErrors { get; set; }
        private bool DisableSubmit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            TimeKeepUser = await UsersService.GetUserDetails();
            var vacationCarriedOverEntry = await PTOEntriesService.GetVacationCarriedOverEntryByUserId(TimeKeepUser.Id);

            VacationCarriedOver = vacationCarriedOverEntry is null ? 0 : vacationCarriedOverEntry.PTOHours / 8;
            
            User = new UserVM
            {
                Id = TimeKeepUser.Id,
                FirstName = TimeKeepUser.FirstName,
                LastName = TimeKeepUser.LastName,
                HireDate = TimeKeepUser.HireDate,
                VacationDaysAccruedPerMonth = TimeKeepUser.VacationDaysAccruedPerMonth,
                SickHoursAccruedPerMonth = TimeKeepUser.SickHoursAccruedPerMonth,
                PersonalDaysPerYear = TimeKeepUser.PersonalDaysPerYear
            };
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;

            User.VacationDaysCarriedOver = VacationCarriedOver;

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
