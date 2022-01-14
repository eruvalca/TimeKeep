using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Logic;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Pages.PTOEntries
{
    [Authorize]
    public partial class EditPTOEntry
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private PTOEntriesService PTOEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public int Id { get; set; }

        private TimeKeepUser TimeKeepUser { get; set; }
        private List<PTOEntry> PTOEntries { get; set; }
        private decimal? VacationHoursAvailable { get; set; }
        private decimal? SickHoursAvailable { get; set; }
        private decimal? PersonalHoursAvailable { get; set; }
        private decimal SelectedHoursAvailable { get; set; }
        private PTOEntry PTOEntry { get; set; }
        private DateTime _ptoDate;
        [Required]
        private DateTime PTODate
        {
            get { return _ptoDate; }
            set
            {
                _ptoDate = value;
                GetAvailablePTOHoursByDate(_ptoDate);
            }
        }
        private PTOType _ptoType;
        [Required]
        private PTOType PTOType
        {
            get { return _ptoType; }
            set
            {
                _ptoType = value;
                UpdateSelectedHoursAvailable(_ptoType);
            }
        }
        private bool DisableSubmit { get; set; }
        private bool DisableDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            TimeKeepUser = await UsersService.GetUserDetails();
            PTOEntries = await PTOEntriesService.GetPTOEntriesByUserId(TimeKeepUser.Id);
            PTOEntry = await PTOEntriesService.GetPTOEntryById(Id);

            PTOEntries = PTOEntries.Where(p => p.PTOEntryId != Id).ToList();

            if (PTOEntry.TimeKeepUserId != TimeKeepUser.Id)
            {
                Navigation.NavigateTo("/");
            }

            PTOEntry.PTOHours *= -1;
            PTODate = PTOEntry.PTODate.ToLocalTime();
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;

            PTOEntry.PTODate = PTODate;
            await PTOEntriesService.UpdatePTOEntry(Id, PTOEntry);

            Navigation.NavigateTo("/");
        }

        private async Task HandleDelete()
        {
            DisableDelete = true;

            await PTOEntriesService.DeletePTOEntry(Id);

            Navigation.NavigateTo("/");
        }

        private void GetAvailablePTOHoursByDate(DateTime date)
        {
            VacationHoursAvailable = PTOCalculator.GetVacationHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);
            SickHoursAvailable = PTOCalculator.GetSickHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);
            PersonalHoursAvailable = PTOCalculator.GetPersonalHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);

            UpdateSelectedHoursAvailable(PTOType);
        }

        private void UpdateSelectedHoursAvailable(PTOType ptoType)
        {
            switch (ptoType)
            {
                case PTOType.Vacation:
                    SelectedHoursAvailable = (decimal)VacationHoursAvailable;
                    break;
                case PTOType.Sick:
                    SelectedHoursAvailable = (decimal)SickHoursAvailable;
                    break;
                case PTOType.Personal:
                    SelectedHoursAvailable = (decimal)PersonalHoursAvailable;
                    break;
                default:
                    SelectedHoursAvailable = 8;
                    break;
            }
        }
    }
}
