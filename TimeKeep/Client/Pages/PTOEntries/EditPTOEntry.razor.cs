using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Logic;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;

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

        private PTOEntryVM PTOEntryVM { get; set; }
        private PTOEntry PTOEntry { get; set; }
        private bool DisableSubmit { get; set; }
        private bool DisableDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            PTOEntryVM = new PTOEntryVM
            {
                TimeKeepUser = await UsersService.GetUserDetails()
            };
            PTOEntryVM.PTOEntries = await PTOEntriesService.GetPTOEntriesByUserId(PTOEntryVM.TimeKeepUser.Id);
            PTOEntryVM.TimeKeepUserId = PTOEntryVM.TimeKeepUser.Id;

            PTOEntry = await PTOEntriesService.GetPTOEntryById(Id);

            PTOEntryVM.PTODate = PTOEntry.PTODate.ToLocalTime();
            PTOEntryVM.CreateDate = PTOEntry.CreateDate.ToLocalTime();
            PTOEntryVM.PTOType = PTOEntry.PTOType;
            PTOEntryVM.PTOHours = PTOEntry.PTOHours * -1;

            PTOEntryVM.PTOEntries = PTOEntryVM.PTOEntries.Where(p => p.PTOEntryId != Id).ToList();

            if (PTOEntry.TimeKeepUserId != PTOEntryVM.TimeKeepUser.Id)
            {
                Navigation.NavigateTo("/");
            }
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;

            PTOEntry.PTODate = PTOEntryVM.PTODate;
            PTOEntry.PTOHours = PTOEntryVM.PTOHours;
            PTOEntry.PTOType = PTOEntryVM.PTOType;
            PTOEntry.CreateDate = PTOEntryVM.CreateDate;
            PTOEntry.ModifyDate = DateTime.Now;
            PTOEntry.TimeKeepUserId = PTOEntryVM.TimeKeepUserId;

            await PTOEntriesService.UpdatePTOEntry(Id, PTOEntry);

            Navigation.NavigateTo("/");
        }

        private async Task HandleDelete()
        {
            DisableDelete = true;

            await PTOEntriesService.DeletePTOEntry(Id);

            Navigation.NavigateTo("/");
        }
    }
}
