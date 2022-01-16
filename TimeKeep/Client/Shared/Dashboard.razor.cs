using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Logic;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Client.Shared
{
    [Authorize]
    public partial class Dashboard
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private PTOEntriesService PTOEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private TimeKeepUser TimeKeepUser { get; set; }
        private List<PTOEntry> PTOEntries { get; set; }
        private decimal? VacationHoursAvailable { get; set; }
        public decimal? SickHoursAvailable { get; set; }
        public decimal? PersonalHoursAvailable { get; set; }
        private decimal? VacationHoursPlanned { get; set; }
        public decimal? SickHoursPlanned { get; set; }
        public decimal? PersonalHoursPlanned { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TimeKeepUser = await UsersService.GetUserDetails();
            PTOEntries = await PTOEntriesService.GetPTOEntriesByUserId(TimeKeepUser.Id);

            VacationHoursAvailable = PTOCalculator.GetVacationHoursAvailableByDate(TimeKeepUser, PTOEntries, DateTime.Today);
            SickHoursAvailable = PTOCalculator.GetSickHoursAvailableByDate(TimeKeepUser, PTOEntries, DateTime.Today);
            PersonalHoursAvailable = PTOCalculator.GetPersonalHoursAvailableByDate(TimeKeepUser, PTOEntries, DateTime.Today);

            VacationHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, DateTime.Today, PTOType.Vacation) * -1);
            SickHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, DateTime.Today, PTOType.Sick) * -1);
            PersonalHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, DateTime.Today, PTOType.Personal) * -1);
        }
    }
}
