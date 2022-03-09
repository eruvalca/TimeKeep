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
        private HolidaysService HolidaysService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private TimeKeepUser TimeKeepUser { get; set; }
        private List<PTOEntry> PTOEntries { get; set; }
        private List<Holiday> Holidays { get; set; }
        private decimal? VacationHoursAvailable { get; set; }
        public decimal? SickHoursAvailable { get; set; }
        public decimal? PersonalHoursAvailable { get; set; }
        private decimal? VacationHoursPlanned { get; set; }
        public decimal? SickHoursPlanned { get; set; }
        public decimal? PersonalHoursPlanned { get; set; }
        public decimal? VacationHoursCarriedOverRemaining { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var today = DateTime.Today;

            TimeKeepUser = await UsersService.GetUserDetails();
            PTOEntries = await PTOEntriesService.GetPTOEntriesByUserId(TimeKeepUser.Id);
            Holidays = await HolidaysService.GetHolidaysByYear(today.Year);

            VacationHoursAvailable = PTOCalculator.GetVacationHoursAvailableByDate(TimeKeepUser, PTOEntries, today);
            SickHoursAvailable = PTOCalculator.GetSickHoursAvailableByDate(TimeKeepUser, PTOEntries, today);
            PersonalHoursAvailable = PTOCalculator.GetPersonalHoursAvailableByDate(TimeKeepUser, PTOEntries, today);

            VacationHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, today, PTOType.Vacation) * -1);
            SickHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, today, PTOType.Sick) * -1);
            PersonalHoursPlanned = (PTOCalculator.GetHoursPlannedAfterDateByType(PTOEntries, today, PTOType.Personal) * -1);

            if (today < new DateTime(DateTime.Now.Year, 4, 1))
            {
                VacationHoursCarriedOverRemaining = (PTOCalculator.GetVacationHoursCarriedOverRemaining(PTOEntries) * -1);
            }
        }
    }
}
