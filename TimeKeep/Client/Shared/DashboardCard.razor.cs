using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace TimeKeep.Client.Shared
{
    [Authorize]
    public partial class DashboardCard
    {
        [Parameter]
        public string PTOType { get; set; }
        [Parameter]
        public decimal HoursAvailable { get; set; }
        [Parameter]
        public decimal HoursPlanned { get; set; }
    }
}
