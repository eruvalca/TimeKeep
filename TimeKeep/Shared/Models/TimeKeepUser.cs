using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Shared.Models
{
    public class TimeKeepUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal VacationDaysAccruedPerMonth { get; set; } = 1.25M;
        public decimal SickHoursAccruedPerMonth { get; set; } = 5.33M;
        public int PersonalDaysPerYear { get; set; } = 3;

        public string GetFullName()
        {
            return string.Concat(FirstName, " ", LastName);
        }
    }
}
