using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Logic;
using System.ComponentModel.DataAnnotations;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Client.Pages.PTOEntries
{
    [Authorize]
    public partial class CreatePTOEntry
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private PTOEntriesService PTOEntriesService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private PTOEntryVM PTOEntryVM { get; set; }
        private bool DisableSubmit { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PTOEntryVM = new PTOEntryVM
            {
                TimeKeepUser = await UsersService.GetUserDetails()
            };
            PTOEntryVM.PTOEntries = await PTOEntriesService.GetPTOEntriesByUserId(PTOEntryVM.TimeKeepUser.Id);
            PTOEntryVM.TimeKeepUserId = PTOEntryVM.TimeKeepUser.Id;
            PTOEntryVM.PTODate = DateTime.Today;
            PTOEntryVM.PTOType = PTOType.Vacation;
            PTOEntryVM.PTOHours = 1;
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;

            var ptoEntry = new PTOEntry
            {
                PTODate = PTOEntryVM.PTODate,
                PTOHours = PTOEntryVM.PTOHours,
                PTOType = PTOEntryVM.PTOType,
                CreateDate = DateTime.Now,
                ModifyDate = PTOEntryVM.ModifyDate,
                TimeKeepUserId = PTOEntryVM.TimeKeepUserId
            };

            ptoEntry = await PTOEntriesService.CreatePTOEntry(ptoEntry);

            Navigation.NavigateTo("/");
        }
    }
}
