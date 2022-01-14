using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeep.Shared.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime HireDate { get; set; }
        [Required]
        public decimal VacationDaysAccruedPerMonth { get; set; }
        [Required]
        public decimal SickHoursAccruedPerMonth { get; set; }
        [Required]
        public int PersonalDaysPerYear { get; set; }
        public decimal VacationDaysCarriedOver { get; set; }
    }
}
