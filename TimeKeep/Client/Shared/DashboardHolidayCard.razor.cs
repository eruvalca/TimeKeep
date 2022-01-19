using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Linq;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Shared
{
    [Authorize]
    public partial class DashboardHolidayCard
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public List<Holiday> Holidays { get; set; }
        private DateTime Today { get; set; } = DateTime.Today;
        private Holiday NextHoliday { get; set; }
        private int RemainingHolidays { get; set; }

        protected override void OnParametersSet()
        {
            NextHoliday = Holidays.Where(h => h.Date.Date >= Today.Date).OrderBy(h => h.Date).FirstOrDefault();
            RemainingHolidays = Holidays.Where(h => h.Date.Date >= Today.Date).Count();
        }
    }
}
